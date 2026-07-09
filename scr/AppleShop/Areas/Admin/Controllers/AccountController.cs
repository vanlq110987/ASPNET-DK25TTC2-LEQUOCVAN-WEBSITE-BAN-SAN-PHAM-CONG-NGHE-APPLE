using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AppleShop.Models;
using AppleShop.Models.ViewModels;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>Quản lý tài khoản người dùng: khóa/mở khóa, gán/thu hồi vai trò (RBAC).</summary>
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

        // POST: /Admin/Account/ToggleLock — khóa / mở khóa tài khoản
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

        // GET: /Admin/Account/EditRoles/{id}
        public ActionResult EditRoles(string id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            var userRoles = UserManager.GetRoles(user.Id);
            ViewBag.AllRoles = db.Roles.Select(r => r.Name).OrderBy(n => n).ToList();
            ViewBag.User = user;

            return View((List<string>)userRoles.ToList());
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
    }
}
