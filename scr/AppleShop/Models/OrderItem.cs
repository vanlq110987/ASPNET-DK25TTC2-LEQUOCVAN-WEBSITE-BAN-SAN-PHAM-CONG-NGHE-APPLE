using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Dòng sản phẩm trong đơn hàng — lưu snapshot tên/giá tại thời điểm đặt.</summary>
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int VariantId { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Sản phẩm")]
        public string ProductName { get; set; }

        [StringLength(200)]
        [Display(Name = "Phiên bản")]
        public string VariantDesc { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual ProductVariant Variant { get; set; }

        [NotMapped]
        public decimal LineTotal => UnitPrice * Quantity;
    }
}
