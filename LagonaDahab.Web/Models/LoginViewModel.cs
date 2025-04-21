using System.ComponentModel.DataAnnotations;

namespace LagonaDahab.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public  string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public  string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string? RedirectUrl { get; set; }
 

    }
}
