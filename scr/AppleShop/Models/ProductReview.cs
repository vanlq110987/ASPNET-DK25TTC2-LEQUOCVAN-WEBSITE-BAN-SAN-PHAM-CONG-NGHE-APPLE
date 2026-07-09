using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Bình luận và đánh giá sản phẩm của khách hàng.</summary>
    [Table("ProductReviews")]
    public class ProductReview
    {
        [Key]
        public int ReviewId { get; set; }

        public int ProductId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        [StringLength(100)]
        [Display(Name = "Tên người đánh giá")]
        public string ReviewerName { get; set; }

        [Range(1, 5)]
        [Display(Name = "Đánh giá (sao)")]
        public int Rating { get; set; } = 5;

        [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận")]
        [StringLength(1000)]
        [Display(Name = "Bình luận")]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = true;

        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
