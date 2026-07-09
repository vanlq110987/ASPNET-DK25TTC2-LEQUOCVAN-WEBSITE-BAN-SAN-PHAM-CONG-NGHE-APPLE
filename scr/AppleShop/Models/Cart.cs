using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    /// <summary>Giỏ hàng lưu CSDL (theo UserId hoặc SessionId).</summary>
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(100)]
        public string SessionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }
    }
}
