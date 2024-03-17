using System.ComponentModel.DataAnnotations;

namespace API.Param
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public string? FirebaseRegisterToken { get; set; }

    }
}
