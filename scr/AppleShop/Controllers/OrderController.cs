using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AppleShop.Models;
using AppleShop.Models.ViewModels;

namespace AppleShop.Controllers
{
    public class OrderController : Controller
    {
        private const string CartSessionKey = "CART";
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private List<SessionCartItem> GetCart()
        {
            return Session[CartSessionKey] as List<SessionCartItem> ?? new List<SessionCartItem>();
        }

        // GET: /Order/Checkout
        public ActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["CartMessage"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel { Items = cart };

            // Tự điền thông tin nếu đã đăng nhập
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user != null)
                {
                    model.FullName = user.FullName;
                    model.Email = user.Email;
                    model.Phone = user.PhoneNumber;
                    model.Address = user.Address;
                }
            }

            return View(model);
        }

        // POST: /Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModel model)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["CartMessage"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            model.Items = cart;
            if (!ModelState.IsValid) return View(model);

            var userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var customer = new Customer
                    {
                        UserId = userId,
                        FullName = model.FullName.Trim(),
                        Phone = model.Phone.Trim(),
                        Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim(),
                        Address = model.Address.Trim(),
                        CreatedAt = DateTime.Now
                    };
                    db.Customers.Add(customer);
                    db.SaveChanges();

                    var total = cart.Sum(i => i.LineTotal);
                    var order = new Order
                    {
                        OrderCode = "AS" + DateTime.Now.ToString("yyMMddHHmmss"),
                        UserId = userId,
                        CustomerId = customer.CustomerId,
                        OrderDate = DateTime.Now,
                        Status = OrderStatus.New,
                        TotalAmount = total,
                        Note = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note.Trim(),
                        IsRead = false
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    foreach (var item in cart)
                    {
                        db.OrderItems.Add(new OrderItem
                        {
                            OrderId = order.OrderId,
                            VariantId = item.VariantId,
                            ProductName = item.ProductName,
                            VariantDesc = item.VariantDesc,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        });
                    }

                    db.Payments.Add(new Payment
                    {
                        OrderId = order.OrderId,
                        Method = "COD",
                        Amount = total,
                        Status = PaymentStatus.Unpaid
                    });

                    db.SaveChanges();
                    transaction.Commit();

                    Session[CartSessionKey] = null; // xóa giỏ hàng sau khi đặt thành công
                    return RedirectToAction("Success", new { code = order.OrderCode });
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.");
                    return View(model);
                }
            }
        }

        // GET: /Order/Success?code=AS...
        public ActionResult Success(string code)
        {
            var order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefault(o => o.OrderCode == code);

            if (order == null) return RedirectToAction("Index", "Home");
            return View(order);
        }

        // GET: /Order/History — lịch sử mua hàng theo tài khoản đăng nhập
        [Authorize]
        public ActionResult History()
        {
            var userId = User.Identity.GetUserId();
            var orders = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // GET: /Order/Details/5 — chi tiết đơn của chính mình
        [Authorize]
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .Include(o => o.Payments)
                .FirstOrDefault(o => o.OrderId == id && o.UserId == userId);

            if (order == null) return HttpNotFound();
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
