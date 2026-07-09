using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AppleShop.Models;
using AppleShop.Models.ViewModels;

namespace AppleShop.Areas.Admin.Controllers
{
    /// <summary>Dashboard tổng quan: đơn hàng mới, doanh thu, sản phẩm bán chạy.</summary>
    public class HomeController : AdminControllerBase
    {
        public ActionResult Index()
        {
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-6);

            var model = new DashboardViewModel
            {
                NewOrderCount = db.Orders.Count(o => !o.IsRead),
                TotalOrders = db.Orders.Count(),
                TotalProducts = db.Products.Count(p => p.IsActive),
                TotalUsers = db.Users.Count(),
                TotalRevenue = db.Orders
                    .Where(o => o.Status == OrderStatus.Delivered)
                    .Select(o => (decimal?)o.TotalAmount).Sum() ?? 0,

                LatestOrders = db.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(8).ToList(),

                TopProducts = db.OrderItems
                    .Where(i => i.Order.Status != OrderStatus.Cancelled)
                    .GroupBy(i => i.ProductName)
                    .Select(g => new TopProductItem
                    {
                        ProductName = g.Key,
                        QuantitySold = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
                    })
                    .OrderByDescending(t => t.QuantitySold)
                    .Take(5).ToList()
            };

            // Doanh thu 7 ngày gần nhất (đơn không bị hủy)
            var revenueRaw = db.Orders
                .Where(o => o.OrderDate >= sevenDaysAgo && o.Status != OrderStatus.Cancelled)
                .GroupBy(o => DbFunctions.TruncateTime(o.OrderDate))
                .Select(g => new { Day = g.Key, Amount = g.Sum(o => o.TotalAmount) })
                .ToList();

            for (var day = sevenDaysAgo; day <= today; day = day.AddDays(1))
            {
                var found = revenueRaw.FirstOrDefault(r => r.Day == day);
                model.RevenueLast7Days.Add(new RevenuePoint
                {
                    Label = day.ToString("dd/MM"),
                    Amount = found != null ? found.Amount : 0
                });
            }

            return View(model);
        }

        // GET: /Admin/Home/NewOrdersCount — AJAX polling cho thông báo đơn hàng mới
        public JsonResult NewOrdersCount()
        {
            var count = db.Orders.Count(o => !o.IsRead);
            return Json(new { count }, JsonRequestBehavior.AllowGet);
        }
    }
}
