using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Biến thể sản phẩm: màu sắc, dung lượng, cấu hình.</summary>
    [Table("ProductVariants")]
    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }

        public int ProductId { get; set; }

        [StringLength(50)]
        [Display(Name = "Màu sắc")]
        public string Color { get; set; }

        [StringLength(50)]
        [Display(Name = "Dung lượng")]
        public string Storage { get; set; }

        [StringLength(255)]
        [Display(Name = "Cấu hình")]
        public string SpecSummary { get; set; }

        [Display(Name = "Chênh lệch giá (VNĐ)")]
        public decimal PriceAdjustment { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Mã SKU")]
        public string Sku { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        public virtual Product Product { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(Color)) parts.Add(Color);
                if (!string.IsNullOrEmpty(Storage)) parts.Add(Storage);
                return string.Join(" · ", parts);
            }
        }
    }
}
