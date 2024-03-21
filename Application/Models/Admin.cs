namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Admin
    {
        [Key]
        public string AdminEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
