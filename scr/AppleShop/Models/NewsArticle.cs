using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace AppleShop.Models
{
    /// <summary>Bài viết tin tức / khuyến mãi — nội dung HTML soạn bằng CKEditor.</summary>
    [Table("NewsArticles")]
    public class NewsArticle
    {
        [Key]
        public int ArticleId { get; set; }

        [Display(Name = "Chủ đề")]
        public int NewsCategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(255)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required, StringLength(270)]
        public string Slug { get; set; }

        [StringLength(500)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [StringLength(500)]
        [Display(Name = "Ảnh đại diện")]
        public string ThumbnailUrl { get; set; }

        [StringLength(100)]
        [Display(Name = "Tác giả")]
        public string AuthorName { get; set; }

        public int ViewCount { get; set; }

        [Display(Name = "Xuất bản")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public virtual NewsCategory NewsCategory { get; set; }
    }
}
