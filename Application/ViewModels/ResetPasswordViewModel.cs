using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Code { get; set; }

        [Required]
        [EmailAddress]
        public string ConsumerEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ReTypePassword { get; set; }
    }
}
