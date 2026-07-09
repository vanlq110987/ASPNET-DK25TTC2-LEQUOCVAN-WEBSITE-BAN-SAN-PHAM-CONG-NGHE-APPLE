using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Chủ đề tin tức: Khuyến mãi, Tin công nghệ, Thủ thuật...</summary>
    [Table("NewsCategories")]
    public class NewsCategory
    {
        [Key]
        public int NewsCategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên chủ đề")]
        [StringLength(100)]
        [Display(Name = "Tên chủ đề")]
        public string Name { get; set; }

        [Required, StringLength(120)]
        public string Slug { get; set; }

        public virtual ICollection<NewsArticle> Articles { get; set; }
    }
}
