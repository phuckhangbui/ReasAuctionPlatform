using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class DepositPaymentDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ReasId { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        public int Money { get; set; }

    }
}
