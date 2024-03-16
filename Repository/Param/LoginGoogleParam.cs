using System.ComponentModel.DataAnnotations;

namespace Repository.Param
{
    public class LoginGoogleParam
    {
        [Required]
        public string IdTokenString { get; set; }

        public string? FirebaseRegisterToken { get; set; }
    }
}
