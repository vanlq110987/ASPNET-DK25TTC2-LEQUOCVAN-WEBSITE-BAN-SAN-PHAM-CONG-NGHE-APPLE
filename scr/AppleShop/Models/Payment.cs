using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleShop.Models
{
    public enum PaymentStatus : byte
    {
        [Display(Name = "Chưa thanh toán")] Unpaid = 0,
        [Display(Name = "Đã thanh toán")] Paid = 1
    }

    /// <summary>Thanh toán của đơn hàng (hiện hỗ trợ COD).</summary>
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        [Required, StringLength(20)]
        [Display(Name = "Phương thức")]
        public string Method { get; set; } = "COD";

        [Display(Name = "Số tiền")]
        public decimal Amount { get; set; }

        [Display(Name = "Trạng thái")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid;

        [Display(Name = "Thanh toán lúc")]
        public DateTime? PaidAt { get; set; }

        public virtual Order Order { get; set; }
    }
}
