using System.Collections.Generic;
using AppleShop.Models;

namespace AppleShop.Models.ViewModels
{
    public class TopProductItem
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RevenuePoint
    {
        public string Label { get; set; }
        public decimal Amount { get; set; }
    }

    /// <summary>Số liệu tổng quan cho Dashboard Admin.</summary>
    public class DashboardViewModel
    {
        public int NewOrderCount { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<Order> LatestOrders { get; set; } = new List<Order>();
        public List<TopProductItem> TopProducts { get; set; } = new List<TopProductItem>();
        public List<RevenuePoint> RevenueLast7Days { get; set; } = new List<RevenuePoint>();
    }
}
