using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppleShop.Helpers;
using AppleShop.Models;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>CRUD Sản phẩm công nghệ Apple: tìm kiếm, phân trang, upload ảnh, xóa mềm.</summary>
    public class ProductController : AdminControllerBase
    {
        private const int PageSize = 10;

        // GET: /Admin/Product?q=iphone&page=1
        public ActionResult Index(string q, int? categoryId, int page = 1)
        {
            var query = db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Name.Contains(q));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            var total = query.Count();
            if (page < 1) page = 1;

            ViewBag.Keyword = q;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = db.Categories.OrderBy(c => c.DisplayOrder).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPages = (total + PageSize - 1) / PageSize;

            var products = query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(products);
        }

        // GET: /Admin/Product/Create
        public ActionResult Create()
        {
            PopulateCategories();
            return View(new Product());
        }

        // POST: /Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model, HttpPostedFileBase imageFile)
        {
            PopulateCategories();
            model.Slug = EnsureUniqueSlug(SlugHelper.ToSlug(model.Name), null);

            ModelState.Remove("Slug");
            if (!ModelState.IsValid) return View(model);

            var uploadedUrl = SaveImage(imageFile);
            if (uploadedUrl != null) model.ImageUrl = uploadedUrl;

            model.CreatedAt = DateTime.Now;
            db.Products.Add(model);
            db.SaveChanges();

            TempData["Success"] = "Đã thêm sản phẩm \"" + model.Name + "\".";
            return RedirectToAction("Index");
        }

        // GET: /Admin/Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            PopulateCategories();
            return View(product);
        }

        // POST: /Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, HttpPostedFileBase imageFile)
        {
            PopulateCategories();

            var product = db.Products.Find(model.ProductId);
            if (product == null) return HttpNotFound();

            ModelState.Remove("Slug");
            if (!ModelState.IsValid) return View(model);

            product.CategoryId = model.CategoryId;
            product.Name = model.Name;
            product.Slug = EnsureUniqueSlug(SlugHelper.ToSlug(model.Name), product.ProductId);
            product.ShortDescription = model.ShortDescription;
            product.Description = model.Description;
            product.Price = model.Price;
            product.SalePrice = model.SalePrice;
            product.Need = model.Need;
            product.IsFeatured = model.IsFeatured;
            product.IsActive = model.IsActive;
            product.UpdatedAt = DateTime.Now;

            var uploadedUrl = SaveImage(imageFile);
            if (uploadedUrl != null)
                product.ImageUrl = uploadedUrl;
            else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
                product.ImageUrl = model.ImageUrl;

            db.SaveChanges();

            TempData["Success"] = "Đã cập nhật sản phẩm \"" + product.Name + "\".";
            return RedirectToAction("Index");
        }

        // POST: /Admin/Product/ToggleActive/5 — xóa mềm / kinh doanh lại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleActive(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            product.IsActive = !product.IsActive;
            product.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            TempData["Success"] = product.IsActive
                ? "Đã mở bán lại \"" + product.Name + "\"."
                : "Đã ngừng kinh doanh (xóa mềm) \"" + product.Name + "\".";
            return RedirectToAction("Index");
        }

        // POST: /Admin/Product/Delete/5 — xóa vĩnh viễn (chỉ khi chưa phát sinh đơn hàng)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var product = db.Products.Include(p => p.Variants).FirstOrDefault(p => p.ProductId == id);
            if (product == null) return HttpNotFound();

            var variantIds = product.Variants.Select(v => v.VariantId).ToList();
            var hasOrders = db.OrderItems.Any(i => variantIds.Contains(i.VariantId));
            if (hasOrders)
            {
                TempData["Error"] = "Không thể xóa vĩnh viễn: sản phẩm đã phát sinh đơn hàng. Hãy dùng xóa mềm.";
                return RedirectToAction("Index");
            }

            db.Products.Remove(product); // variants/images/reviews xóa theo cascade
            db.SaveChanges();

            TempData["Success"] = "Đã xóa vĩnh viễn \"" + product.Name + "\".";
            return RedirectToAction("Index");
        }

        private void PopulateCategories()
        {
            ViewBag.Categories = new SelectList(db.Categories.OrderBy(c => c.DisplayOrder).ToList(), "CategoryId", "Name");
        }

        /// <summary>Lưu ảnh upload vào ~/Content/uploads, trả về đường dẫn tương đối (null nếu không upload).</summary>
        private string SaveImage(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0) return null;

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".webp")
            {
                ModelState.AddModelError("", "Chỉ chấp nhận ảnh .jpg, .png hoặc .webp");
                return null;
            }

            var folder = Server.MapPath("~/Content/uploads");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" +
                           SlugHelper.ToSlug(Path.GetFileNameWithoutExtension(file.FileName)) + ext;
            file.SaveAs(Path.Combine(folder, fileName));
            return "/Content/uploads/" + fileName;
        }

        /// <summary>Bảo đảm slug không trùng — nếu trùng thì thêm hậu tố -2, -3, ...</summary>
        private string EnsureUniqueSlug(string slug, int? excludeProductId)
        {
            var candidate = slug;
            var counter = 2;
            while (db.Products.Any(p => p.Slug == candidate &&
                                        (!excludeProductId.HasValue || p.ProductId != excludeProductId.Value)))
            {
                candidate = slug + "-" + counter++;
            }
            return candidate;
        }
    }
}
