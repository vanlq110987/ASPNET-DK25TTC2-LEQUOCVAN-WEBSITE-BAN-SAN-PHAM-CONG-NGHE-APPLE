using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Tồn kho theo biến thể sản phẩm × kênh phân phối.</summary>
    [Table("Inventories")]
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int VariantId { get; set; }

        public int ChannelId { get; set; }

        [Display(Name = "Số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ")]
        public int Quantity { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ProductVariant Variant { get; set; }
        public virtual DistributionChannel Channel { get; set; }
    }
}
