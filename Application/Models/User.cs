namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public string ConsumerEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
