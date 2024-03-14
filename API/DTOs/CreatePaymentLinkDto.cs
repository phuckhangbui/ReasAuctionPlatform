using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreatePaymentLinkDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int ReasId { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }


}
