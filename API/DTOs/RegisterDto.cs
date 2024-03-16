using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string AccountName { get; set; }
        [EmailAddress]
        public string AccountEmail { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
                                    ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alpha numeric character and at least 6 characters")]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
