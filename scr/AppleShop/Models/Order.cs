using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    public enum OrderStatus : byte
    {
        [Display(Name = "Mới")] New = 0,
        [Display(Name = "Đang xử lý")] Processing = 1,
        [Display(Name = "Đã giao")] Delivered = 2,
        [Display(Name = "Hủy")] Cancelled = 3
    }

    /// <summary>Đơn hàng.</summary>
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required, StringLength(20)]
        [Display(Name = "Mã đơn hàng")]
        public string OrderCode { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public OrderStatus Status { get; set; } = OrderStatus.New;

        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        /// <summary>Admin đã xem đơn chưa — phục vụ thông báo đơn hàng mới.</summary>
        public bool IsRead { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        [NotMapped]
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case OrderStatus.New: return "Mới";
                    case OrderStatus.Processing: return "Đang xử lý";
                    case OrderStatus.Delivered: return "Đã giao";
                    case OrderStatus.Cancelled: return "Hủy";
                    default: return Status.ToString();
                }
            }
        }

        [NotMapped]
        public string StatusBadgeClass
        {
            get
            {
                switch (Status)
                {
                    case OrderStatus.New: return "badge-primary";
                    case OrderStatus.Processing: return "badge-warning";
                    case OrderStatus.Delivered: return "badge-success";
                    case OrderStatus.Cancelled: return "badge-danger";
                    default: return "badge-secondary";
                }
            }
        }
    }
}
