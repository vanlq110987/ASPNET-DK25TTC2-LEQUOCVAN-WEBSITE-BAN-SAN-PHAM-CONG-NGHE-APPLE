using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AppleShop.Models;

namespace AppleShop.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private const int PageSize = 6;

        // GET: /News?category=khuyen-mai&page=1
        public ActionResult Index(string category, int page = 1)
        {
            var query = db.NewsArticles
                .Include(a => a.NewsCategory)
                .Where(a => a.IsPublished);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(a => a.NewsCategory.Slug == category);

            var total = query.Count();
            if (page < 1) page = 1;

            ViewBag.Categories = db.NewsCategories.OrderBy(c => c.Name).ToList();
            ViewBag.CurrentCategory = category;
            ViewBag.Page = page;
            ViewBag.TotalPages = (total + PageSize - 1) / PageSize;

            var articles = query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(articles);
        }

        // GET: /tin-tuc/{slug}
        public ActionResult Detail(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return RedirectToAction("Index");

            var article = db.NewsArticles
                .Include(a => a.NewsCategory)
                .FirstOrDefault(a => a.Slug == slug && a.IsPublished);

            if (article == null) return HttpNotFound();

            article.ViewCount++;
            db.SaveChanges();

            ViewBag.RelatedArticles = db.NewsArticles
                .Where(a => a.NewsCategoryId == article.NewsCategoryId
                         && a.ArticleId != article.ArticleId && a.IsPublished)
                .OrderByDescending(a => a.CreatedAt)
                .Take(4).ToList();

            return View(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
