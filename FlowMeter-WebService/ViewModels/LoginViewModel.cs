using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string ConsumerEmail { get; set; }

        [Required(ErrorMessage = "Incorrect password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
