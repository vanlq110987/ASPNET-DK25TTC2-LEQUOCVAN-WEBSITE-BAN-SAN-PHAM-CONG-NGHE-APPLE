using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace AppleShop.Models
{
    /// <summary>Sản phẩm công nghệ Apple.</summary>
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Required, StringLength(220)]
        public string Slug { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả ngắn")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [Display(Name = "Mô tả chi tiết")]
        public string Description { get; set; }

        [Display(Name = "Giá bán (VNĐ)")]
        [Range(0, 999999999, ErrorMessage = "Giá không hợp lệ")]
        public decimal Price { get; set; }

        [Display(Name = "Giá khuyến mãi (VNĐ)")]
        public decimal? SalePrice { get; set; }

        [StringLength(500)]
        [Display(Name = "Ảnh đại diện")]
        public string ImageUrl { get; set; }

        [StringLength(50)]
        [Display(Name = "Nhu cầu sử dụng")]
        public string Need { get; set; }

        [Display(Name = "Sản phẩm nổi bật")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Đang kinh doanh")]
        public bool IsActive { get; set; } = true;

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ProductVariant> Variants { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
        public virtual ICollection<ProductReview> Reviews { get; set; }

        /// <summary>Giá thực bán: ưu tiên giá khuyến mãi nếu có.</summary>
        [NotMapped]
        public decimal EffectivePrice => SalePrice.HasValue && SalePrice.Value > 0 ? SalePrice.Value : Price;
    }
}
