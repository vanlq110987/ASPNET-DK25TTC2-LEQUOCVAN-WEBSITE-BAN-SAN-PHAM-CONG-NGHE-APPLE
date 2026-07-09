using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AppleShop.Models;

namespace AppleShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.FeaturedProducts = db.Products
                .Where(p => p.IsActive && p.IsFeatured)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8).ToList();

            ViewBag.NewestProducts = db.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8).ToList();

            ViewBag.LatestNews = db.NewsArticles
                .Include(a => a.NewsCategory)
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.CreatedAt)
                .Take(3).ToList();

            ViewBag.Categories = db.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder).ToList();

            return View();
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
