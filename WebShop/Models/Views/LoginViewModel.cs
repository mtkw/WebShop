using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models.Views
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ErrorMsg { get; set; }
    }
}
