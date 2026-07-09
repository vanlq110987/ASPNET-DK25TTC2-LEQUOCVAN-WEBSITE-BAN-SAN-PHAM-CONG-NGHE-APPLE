using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Kênh phân phối: Apple Store, FPT Shop, CellphoneS, TopZone, ...</summary>
    [Table("DistributionChannels")]
    public class DistributionChannel
    {
        [Key]
        public int ChannelId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên kênh phân phối")]
        [StringLength(100)]
        [Display(Name = "Tên kênh phân phối")]
        public string Name { get; set; }

        [Required, StringLength(120)]
        public string Slug { get; set; }

        [StringLength(255)]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [StringLength(20)]
        [Display(Name = "Hotline")]
        public string Hotline { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
