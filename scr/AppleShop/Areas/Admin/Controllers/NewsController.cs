using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AppleShop.Helpers;
using AppleShop.Models;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>Quản lý Tin tức thuộc Chủ đề — soạn nội dung bằng CKEditor.</summary>
    public class NewsController : AdminControllerBase
    {
        private const int PageSize = 10;

        // GET: /Admin/News
        public ActionResult Index(int? categoryId, int page = 1)
        {
            var query = db.NewsArticles.Include(a => a.NewsCategory).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(a => a.NewsCategoryId == categoryId.Value);

            var total = query.Count();
            if (page < 1) page = 1;

            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = db.NewsCategories.OrderBy(c => c.Name).ToList();
            ViewBag.Page = page;
            ViewBag.TotalPages = (total + PageSize - 1) / PageSize;

            var articles = query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(articles);
        }

        // GET: /Admin/News/Create
        public ActionResult Create()
        {
            PopulateCategories();
            return View(new NewsArticle());
        }

        // POST: /Admin/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // nội dung HTML từ CKEditor
        public ActionResult Create(NewsArticle model)
        {
            PopulateCategories();
            model.Slug = EnsureUniqueSlug(SlugHelper.ToSlug(model.Title), null);

            ModelState.Remove("Slug");
            if (!ModelState.IsValid) return View(model);

            model.CreatedAt = DateTime.Now;
            if (string.IsNullOrWhiteSpace(model.AuthorName)) model.AuthorName = "AppleShop";

            db.NewsArticles.Add(model);
            db.SaveChanges();

            TempData["Success"] = "Đã đăng bài viết \"" + model.Title + "\".";
            return RedirectToAction("Index");
        }

        // GET: /Admin/News/Edit/5
        public ActionResult Edit(int id)
        {
            var article = db.NewsArticles.Find(id);
            if (article == null) return HttpNotFound();

            PopulateCategories();
            return View(article);
        }

        // POST: /Admin/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(NewsArticle model)
        {
            PopulateCategories();

            var article = db.NewsArticles.Find(model.ArticleId);
            if (article == null) return HttpNotFound();

            ModelState.Remove("Slug");
            if (!ModelState.IsValid) return View(model);

            article.NewsCategoryId = model.NewsCategoryId;
            article.Title = model.Title;
            article.Slug = EnsureUniqueSlug(SlugHelper.ToSlug(model.Title), article.ArticleId);
            article.Summary = model.Summary;
            article.Content = model.Content;
            article.ThumbnailUrl = model.ThumbnailUrl;
            article.AuthorName = model.AuthorName;
            article.IsPublished = model.IsPublished;
            article.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            TempData["Success"] = "Đã cập nhật bài viết \"" + article.Title + "\".";
            return RedirectToAction("Index");
        }

        // POST: /Admin/News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var article = db.NewsArticles.Find(id);
            if (article == null) return HttpNotFound();

            db.NewsArticles.Remove(article);
            db.SaveChanges();

            TempData["Success"] = "Đã xóa bài viết \"" + article.Title + "\".";
            return RedirectToAction("Index");
        }

        // ===== Chủ đề tin tức =====

        // POST: /Admin/News/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Vui lòng nhập tên chủ đề.";
                return RedirectToAction("Index");
            }

            var slug = SlugHelper.ToSlug(name);
            if (db.NewsCategories.Any(c => c.Slug == slug))
            {
                TempData["Error"] = "Chủ đề \"" + name + "\" đã tồn tại.";
                return RedirectToAction("Index");
            }

            db.NewsCategories.Add(new NewsCategory { Name = name.Trim(), Slug = slug });
            db.SaveChanges();

            TempData["Success"] = "Đã thêm chủ đề \"" + name + "\".";
            return RedirectToAction("Index");
        }

        // POST: /Admin/News/DeleteCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            var category = db.NewsCategories.Find(id);
            if (category == null) return HttpNotFound();

            if (db.NewsArticles.Any(a => a.NewsCategoryId == id))
            {
                TempData["Error"] = "Không thể xóa chủ đề \"" + category.Name + "\": vẫn còn bài viết thuộc chủ đề này.";
                return RedirectToAction("Index");
            }

            db.NewsCategories.Remove(category);
            db.SaveChanges();

            TempData["Success"] = "Đã xóa chủ đề \"" + category.Name + "\".";
            return RedirectToAction("Index");
        }

        private void PopulateCategories()
        {
            ViewBag.Categories = new SelectList(db.NewsCategories.OrderBy(c => c.Name).ToList(), "NewsCategoryId", "Name");
        }

        private string EnsureUniqueSlug(string slug, int? excludeArticleId)
        {
            var candidate = slug;
            var counter = 2;
            while (db.NewsArticles.Any(a => a.Slug == candidate &&
                                            (!excludeArticleId.HasValue || a.ArticleId != excludeArticleId.Value)))
            {
                candidate = slug + "-" + counter++;
            }
            return candidate;
        }
    }
}
