
using System.ComponentModel.DataAnnotations;

namespace DojoSecrets2
{
    public class LoginCheck
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}