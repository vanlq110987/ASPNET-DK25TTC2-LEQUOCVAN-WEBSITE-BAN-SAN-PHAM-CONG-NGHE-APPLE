using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AppleShop.Models;
using AppleShop.Models.ViewModels;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý tài khoản người dùng: thêm/sửa/xóa tài khoản,
    /// khóa/mở khóa, gán/thu hồi vai trò (RBAC).
    /// </summary>
    public class AccountController : AdminControllerBase
    {
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private ApplicationRoleManager RoleManager =>
            HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

        // GET: /Admin/Account
        public ActionResult Index(string q)
        {
            var usersQuery = db.Users.AsQueryable();
            if (!string.IsNullOrEmpty(q))
                usersQuery = usersQuery.Where(u => u.Email.Contains(q) || u.FullName.Contains(q));

            var users = usersQuery.OrderByDescending(u => u.CreatedAt).ToList();
            var roleNames = db.Roles.ToDictionary(r => r.Id, r => r.Name);

            var model = users.Select(u => new UserRolesViewModel
            {
                UserId = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                CreatedAt = u.CreatedAt,
                IsLockedOut = u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow,
                Roles = u.Roles.Select(r => roleNames.ContainsKey(r.RoleId) ? roleNames[r.RoleId] : r.RoleId).ToList()
            }).ToList();

            ViewBag.Keyword = q;
            return View(model);
        }

        // ===== THÊM TÀI KHOẢN =====

        // GET: /Admin/Account/Create
        public ActionResult Create()
        {
            PopulateAllRoles();
            return View(new UserCreateViewModel());
        }

        // POST: /Admin/Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            PopulateAllRoles();
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            // Không chọn vai trò → mặc định Customer
            var roles = (model.Roles != null && model.Roles.Any())
                ? model.Roles
                : new[] { IdentitySeeder.CustomerRole };

            foreach (var role in roles)
            {
                if (RoleManager.RoleExists(role))
                    await UserManager.AddToRoleAsync(user.Id, role);
            }

            TempData["Success"] = "Đã tạo tài khoản " + user.Email + ".";
            return RedirectToAction("Index");
        }

        // ===== SỬA TÀI KHOẢN =====

        // GET: /Admin/Account/Edit/{id}
        public ActionResult Edit(string id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            var model = new UserEditViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
            return View(model);
        }

        // POST: /Admin/Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null) return HttpNotFound();

            user.FullName = model.FullName;
            user.UserName = model.Email; // email dùng làm tên đăng nhập
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            var result = await UserManager.UpdateAsync(user); // UserValidator kiểm tra email trùng
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            // Đổi mật khẩu nếu Admin nhập mật khẩu mới
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (await UserManager.HasPasswordAsync(user.Id))
                    await UserManager.RemovePasswordAsync(user.Id);

                var passResult = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    foreach (var error in passResult.Errors)
                        ModelState.AddModelError("", error);
                    return View(model);
                }
            }

            TempData["Success"] = "Đã cập nhật tài khoản " + user.Email + ".";
            return RedirectToAction("Index");
        }

        // ===== XÓA TÀI KHOẢN =====

        // POST: /Admin/Account/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();

            if (user.Id == User.Identity.GetUserId())
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản của chính mình.";
                return RedirectToAction("Index");
            }

            // Không cho xóa Admin cuối cùng của hệ thống
            if (await UserManager.IsInRoleAsync(user.Id, IdentitySeeder.AdminRole))
            {
                var adminRoleId = db.Roles.Where(r => r.Name == IdentitySeeder.AdminRole)
                                          .Select(r => r.Id).FirstOrDefault();
                var adminCount = db.Users.Count(u => u.Roles.Any(r => r.RoleId == adminRoleId));
                if (adminCount <= 1)
                {
                    TempData["Error"] = "Không thể xóa Admin cuối cùng của hệ thống.";
                    return RedirectToAction("Index");
                }
            }

            var email = user.Email;

            // Gỡ liên kết dữ liệu nghiệp vụ để giữ lịch sử đơn hàng/bình luận (UserId → NULL)
            db.Database.ExecuteSqlCommand("UPDATE dbo.Orders SET UserId = NULL WHERE UserId = {0}", id);
            db.Database.ExecuteSqlCommand("UPDATE dbo.Customers SET UserId = NULL WHERE UserId = {0}", id);
            db.Database.ExecuteSqlCommand("UPDATE dbo.ProductReviews SET UserId = NULL WHERE UserId = {0}", id);
            db.Database.ExecuteSqlCommand("DELETE FROM dbo.Carts WHERE UserId = {0}", id);

            var result = await UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Không xóa được tài khoản: " + string.Join("; ", result.Errors);
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Đã xóa tài khoản " + email + ". Lịch sử đơn hàng liên quan vẫn được giữ lại.";
            return RedirectToAction("Index");
        }

        // ===== KHÓA / MỞ KHÓA =====

        // POST: /Admin/Account/ToggleLock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleLock(string id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            if (user.Id == User.Identity.GetUserId())
            {
                TempData["Error"] = "Bạn không thể tự khóa tài khoản của chính mình.";
                return RedirectToAction("Index");
            }

            var isLocked = user.LockoutEndDateUtc.HasValue && user.LockoutEndDateUtc.Value > DateTime.UtcNow;
            if (isLocked)
            {
                user.LockoutEndDateUtc = null;
                TempData["Success"] = "Đã mở khóa tài khoản " + user.Email + ".";
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100);
                TempData["Success"] = "Đã khóa tài khoản " + user.Email + ".";
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ===== PHÂN QUYỀN =====

        // GET: /Admin/Account/EditRoles/{id}
        public ActionResult EditRoles(string id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            var userRoles = UserManager.GetRoles(user.Id);
            ViewBag.AllRoles = db.Roles.Select(r => r.Name).OrderBy(n => n).ToList();
            ViewBag.User = user;

            return View(userRoles.ToList());
        }

        // POST: /Admin/Account/EditRoles/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(string id, string[] roles)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            roles = roles ?? new string[0];

            // Không cho tự thu hồi quyền Admin của chính mình
            if (user.Id == User.Identity.GetUserId() && !roles.Contains(IdentitySeeder.AdminRole))
            {
                TempData["Error"] = "Bạn không thể tự thu hồi quyền Admin của chính mình.";
                return RedirectToAction("EditRoles", new { id });
            }

            var currentRoles = UserManager.GetRoles(user.Id).ToList();

            foreach (var role in currentRoles.Except(roles))
                UserManager.RemoveFromRole(user.Id, role);

            foreach (var role in roles.Except(currentRoles))
            {
                if (RoleManager.RoleExists(role))
                    UserManager.AddToRole(user.Id, role);
            }

            TempData["Success"] = "Đã cập nhật vai trò cho " + user.Email + ".";
            return RedirectToAction("Index");
        }

        private void PopulateAllRoles()
        {
            ViewBag.AllRoles = db.Roles.Select(r => r.Name).OrderBy(n => n).ToList();
        }
    }
}
