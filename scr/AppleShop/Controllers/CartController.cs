using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AppleShop.Models;
using AppleShop.Models.ViewModels;

namespace AppleShop.Controllers
{
    /// <summary>Giỏ hàng lưu trong Session; cập nhật số lượng qua AJAX (JsonResult).</summary>
    public class CartController : Controller
    {
        private const string CartSessionKey = "CART";
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private List<SessionCartItem> GetCart()
        {
            var cart = Session[CartSessionKey] as List<SessionCartItem>;
            if (cart == null)
            {
                cart = new List<SessionCartItem>();
                Session[CartSessionKey] = cart;
            }
            return cart;
        }

        // GET: /Cart
        public ActionResult Index()
        {
            return View(GetCart());
        }

        // POST: /Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int variantId, int quantity = 1)
        {
            var variant = db.ProductVariants
                .Where(v => v.VariantId == variantId && v.IsActive)
                .Select(v => new
                {
                    v.VariantId,
                    v.ProductId,
                    v.Color,
                    v.Storage,
                    v.PriceAdjustment,
                    ProductName = v.Product.Name,
                    ProductSlug = v.Product.Slug,
                    v.Product.ImageUrl,
                    v.Product.Price,
                    v.Product.SalePrice
                })
                .FirstOrDefault();

            if (variant == null) return HttpNotFound();

            if (quantity < 1) quantity = 1;

            var basePrice = variant.SalePrice.HasValue && variant.SalePrice.Value > 0
                ? variant.SalePrice.Value : variant.Price;
            var unitPrice = basePrice + variant.PriceAdjustment;

            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.VariantId == variantId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                var descParts = new List<string>();
                if (!string.IsNullOrEmpty(variant.Color)) descParts.Add(variant.Color);
                if (!string.IsNullOrEmpty(variant.Storage)) descParts.Add(variant.Storage);

                cart.Add(new SessionCartItem
                {
                    VariantId = variant.VariantId,
                    ProductId = variant.ProductId,
                    ProductName = variant.ProductName,
                    ProductSlug = variant.ProductSlug,
                    VariantDesc = string.Join(" · ", descParts),
                    ImageUrl = variant.ImageUrl,
                    UnitPrice = unitPrice,
                    Quantity = quantity
                });
            }

            TempData["CartMessage"] = "Đã thêm sản phẩm vào giỏ hàng.";
            return RedirectToAction("Index");
        }

        // POST: /Cart/UpdateQuantity — AJAX, trả JSON để cập nhật tổng tiền không reload trang
        [HttpPost]
        public JsonResult UpdateQuantity(int variantId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.VariantId == variantId);
            if (item == null)
                return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng." });

            if (quantity < 1)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            return Json(new
            {
                success = true,
                lineTotal = item != null && quantity >= 1 ? item.LineTotal.ToString("N0") : "0",
                cartTotal = cart.Sum(i => i.LineTotal).ToString("N0"),
                cartCount = cart.Sum(i => i.Quantity)
            });
        }

        // POST: /Cart/Remove — AJAX
        [HttpPost]
        public JsonResult Remove(int variantId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.VariantId == variantId);
            if (item != null) cart.Remove(item);

            return Json(new
            {
                success = true,
                cartTotal = cart.Sum(i => i.LineTotal).ToString("N0"),
                cartCount = cart.Sum(i => i.Quantity),
                isEmpty = !cart.Any()
            });
        }

        // GET: /Cart/Count — số lượng hiển thị trên navbar
        public JsonResult Count()
        {
            var cart = GetCart();
            return Json(new { count = cart.Sum(i => i.Quantity) }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
