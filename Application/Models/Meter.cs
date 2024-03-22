namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Meter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountersId { get; set; }

        public decimal? CurrentIndicator { get; set; }

        [Required]
        public string CounterAccount { get; set; }

        [Required]
        public string TypeOfAccount { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
