namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MeterRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeterRecordId { get; set; }

        [Required]
        public decimal CurrentIndicator { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("MeterId")]
        required public Meter Meter { get; set; }
    }
}
