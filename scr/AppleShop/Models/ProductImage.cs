using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Hình ảnh của sản phẩm hoặc biến thể.</summary>
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        public int? VariantId { get; set; }

        [Required, StringLength(500)]
        public string Url { get; set; }

        public int SortOrder { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductVariant Variant { get; set; }
    }
}
