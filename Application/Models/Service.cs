namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }

        public int? HouseId { get; set; }

        [Required]
        public string TypeOfAccount { get; set; }

        public int? Price { get; set; }

        [ForeignKey(nameof(HouseId))]
        public House House { get; set; }
    }
}
