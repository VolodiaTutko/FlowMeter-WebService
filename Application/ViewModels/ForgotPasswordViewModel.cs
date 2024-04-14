using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter the email")]
        [EmailAddress]
        public string ConsumerEmail { get; set; }
    }
}
