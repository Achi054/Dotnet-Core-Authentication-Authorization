using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(dataType: DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
