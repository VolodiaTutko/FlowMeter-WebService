namespace Application.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : IdentityUser
    {
        [Required]
        public string ConsumerEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
