using System;

namespace AppleShop.Models.ViewModels
{
    /// <summary>Dòng sản phẩm trong giỏ hàng lưu Session.</summary>
    [Serializable]
    public class SessionCartItem
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSlug { get; set; }
        public string VariantDesc { get; set; }
        public string ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
    }
}
