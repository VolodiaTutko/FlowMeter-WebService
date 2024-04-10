using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.ViewModels
{
    public class SignupEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string ConsumerEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-enter your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ReTypePassword { get; set; }
    }
}
