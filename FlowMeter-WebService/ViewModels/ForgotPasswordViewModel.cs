using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string ConsumerEmail { get; set; }
    }
}
