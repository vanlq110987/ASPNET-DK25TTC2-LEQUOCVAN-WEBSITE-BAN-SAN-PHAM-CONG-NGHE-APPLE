using System.Collections.Generic;
using AppleShop.Models;

namespace AppleShop.Models.ViewModels
{
    /// <summary>Dữ liệu trang danh sách sản phẩm + trạng thái bộ lọc.</summary>
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<DistributionChannel> Channels { get; set; } = new List<DistributionChannel>();
        public List<string> Needs { get; set; } = new List<string>();

        // Trạng thái bộ lọc hiện tại
        public string CategorySlug { get; set; }
        public int? ChannelId { get; set; }
        public string Need { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Keyword { get; set; }
        public string Sort { get; set; }

        // Phân trang
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalItems { get; set; }
        public int TotalPages => (TotalItems + PageSize - 1) / PageSize;
    }
}
