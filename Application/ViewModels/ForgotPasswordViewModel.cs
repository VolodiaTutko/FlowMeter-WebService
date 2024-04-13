using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string ConsumerEmail { get; set; }
    }
}
