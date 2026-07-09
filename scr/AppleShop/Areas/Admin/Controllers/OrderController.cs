using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AppleShop.Models;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>Quản lý đơn hàng: lọc theo trạng thái, xem chi tiết, cập nhật trạng thái.</summary>
    public class OrderController : AdminControllerBase
    {
        private const int PageSize = 15;

        // GET: /Admin/Order?status=0&q=AS26&page=1
        public ActionResult Index(byte? status, string q, int page = 1)
        {
            var query = db.Orders.Include(o => o.Customer).AsQueryable();

            if (status.HasValue)
                query = query.Where(o => (byte)o.Status == status.Value);

            if (!string.IsNullOrEmpty(q))
                query = query.Where(o => o.OrderCode.Contains(q)
                                      || o.Customer.FullName.Contains(q)
                                      || o.Customer.Phone.Contains(q));

            var total = query.Count();
            if (page < 1) page = 1;

            ViewBag.Status = status;
            ViewBag.Keyword = q;
            ViewBag.Page = page;
            ViewBag.TotalPages = (total + PageSize - 1) / PageSize;

            var orders = query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(orders);
        }

        // GET: /Admin/Order/Details/5 — mở chi tiết đồng thời đánh dấu đã đọc
        public ActionResult Details(int id)
        {
            var order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .Include(o => o.Payments)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return HttpNotFound();

            if (!order.IsRead)
            {
                order.IsRead = true;
                db.SaveChanges();
            }

            return View(order);
        }

        // POST: /Admin/Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, byte status)
        {
            var order = db.Orders.Include(o => o.Payments).FirstOrDefault(o => o.OrderId == id);
            if (order == null) return HttpNotFound();

            if (!Enum.IsDefined(typeof(OrderStatus), status))
            {
                TempData["Error"] = "Trạng thái không hợp lệ.";
                return RedirectToAction("Details", new { id });
            }

            order.Status = (OrderStatus)status;

            // Đơn đã giao (COD) → ghi nhận thanh toán
            if (order.Status == OrderStatus.Delivered)
            {
                var payment = order.Payments.FirstOrDefault();
                if (payment != null && payment.Status == PaymentStatus.Unpaid)
                {
                    payment.Status = PaymentStatus.Paid;
                    payment.PaidAt = DateTime.Now;
                }
            }

            db.SaveChanges();

            TempData["Success"] = "Đã cập nhật trạng thái đơn " + order.OrderCode + " thành \"" + order.StatusText + "\".";
            return RedirectToAction("Details", new { id });
        }
    }
}
