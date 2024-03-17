using System.ComponentModel.DataAnnotations;

namespace API.Param
{
    public class LoginGoogleParam
    {
        [Required]
        public string IdTokenString { get; set; }

        public string? FirebaseRegisterToken { get; set; }
    }
}
