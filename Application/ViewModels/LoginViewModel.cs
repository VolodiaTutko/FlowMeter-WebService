using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter the email")]
        [EmailAddress]
        public string ConsumerEmail { get; set; }

        [Required(ErrorMessage = "Please enter the password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
