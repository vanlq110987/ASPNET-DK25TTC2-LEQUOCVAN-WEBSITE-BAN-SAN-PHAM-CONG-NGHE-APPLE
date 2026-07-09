using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AppleShop.Models;
using AppleShop.Models.ViewModels;

namespace AppleShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private const int PageSize = 9;

        // GET: /Product?category=iphone&channelId=2&need=Học tập&minPrice=&maxPrice=&q=&sort=&page=1
        public ActionResult Index(string category, int? channelId, string need,
                                  decimal? minPrice, decimal? maxPrice,
                                  string q, string sort, int page = 1)
        {
            var query = db.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Slug == category);

            if (channelId.HasValue)
                query = query.Where(p => p.Variants.Any(v =>
                    v.Inventories.Any(i => i.ChannelId == channelId.Value && i.Quantity > 0)));

            if (!string.IsNullOrEmpty(need))
                query = query.Where(p => p.Need == need);

            if (minPrice.HasValue)
                query = query.Where(p => (p.SalePrice ?? p.Price) >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => (p.SalePrice ?? p.Price) <= maxPrice.Value);

            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Name.Contains(q) || p.ShortDescription.Contains(q));

            switch (sort)
            {
                case "price-asc":
                    query = query.OrderBy(p => p.SalePrice ?? p.Price);
                    break;
                case "price-desc":
                    query = query.OrderByDescending(p => p.SalePrice ?? p.Price);
                    break;
                case "popular":
                    query = query.OrderByDescending(p => p.ViewCount);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            var total = query.Count();
            page = Math.Max(1, page);

            var model = new ProductListViewModel
            {
                Products = query.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                Categories = db.Categories.Where(c => c.IsActive).OrderBy(c => c.DisplayOrder).ToList(),
                Channels = db.DistributionChannels.Where(c => c.IsActive).OrderBy(c => c.Name).ToList(),
                Needs = db.Products.Where(p => p.IsActive && p.Need != null)
                                   .Select(p => p.Need).Distinct().OrderBy(n => n).ToList(),
                CategorySlug = category,
                ChannelId = channelId,
                Need = need,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Keyword = q,
                Sort = sort,
                Page = page,
                PageSize = PageSize,
                TotalItems = total
            };

            return View(model);
        }

        // GET: /san-pham/{slug}
        public ActionResult Detail(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return RedirectToAction("Index");

            var product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Slug == slug && p.IsActive);

            if (product == null) return HttpNotFound();

            product.ViewCount++;
            db.SaveChanges();

            ViewBag.Reviews = db.ProductReviews
                .Where(r => r.ProductId == product.ProductId && r.IsApproved)
                .OrderByDescending(r => r.CreatedAt).ToList();

            ViewBag.RelatedProducts = db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != product.ProductId && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(4).ToList();

            // Kênh phân phối còn hàng cho sản phẩm này
            ViewBag.AvailableChannels = db.Inventories
                .Include(i => i.Channel)
                .Where(i => i.Variant.ProductId == product.ProductId && i.Quantity > 0 && i.Channel.IsActive)
                .Select(i => i.Channel)
                .Distinct().ToList();

            return View(product);
        }

        // POST: /Product/Review — gửi bình luận, đánh giá
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(int productId, string reviewerName, int rating, string comment)
        {
            var product = db.Products.Find(productId);
            if (product == null) return HttpNotFound();

            if (string.IsNullOrWhiteSpace(comment) || rating < 1 || rating > 5)
            {
                TempData["ReviewError"] = "Vui lòng nhập nội dung bình luận và chọn số sao (1–5).";
                return Redirect(Url.RouteUrl("ProductDetail", new { slug = product.Slug }) + "#reviews");
            }

            var review = new ProductReview
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment.Trim(),
                CreatedAt = DateTime.Now
            };

            if (User.Identity.IsAuthenticated)
            {
                review.UserId = User.Identity.GetUserId();
                review.ReviewerName = string.IsNullOrWhiteSpace(reviewerName)
                    ? User.Identity.GetUserName() : reviewerName.Trim();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(reviewerName))
                {
                    TempData["ReviewError"] = "Vui lòng nhập tên của bạn.";
                    return Redirect(Url.RouteUrl("ProductDetail", new { slug = product.Slug }) + "#reviews");
                }
                review.ReviewerName = reviewerName.Trim();
            }

            db.ProductReviews.Add(review);
            db.SaveChanges();

            TempData["ReviewSuccess"] = "Cảm ơn bạn đã gửi đánh giá!";
            return Redirect(Url.RouteUrl("ProductDetail", new { slug = product.Slug }) + "#reviews");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
