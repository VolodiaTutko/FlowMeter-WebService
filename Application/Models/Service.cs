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
        [EnumDataType(typeof(ServiceType))] 
        public string TypeOfAccount { get; set; }

        public int? Price { get; set; }

        [ForeignKey(nameof(HouseId))]
        public House House { get; set; }
    }

    public enum ServiceType
    {
        [Display(Name = "Холодна вода")]
        ColdWater,
        [Display(Name = "Гаряча вода")]
        HotWater,
        [Display(Name = "Газ")]
        Gas,
        [Display(Name = "Світло")]
        Electricity
    }
}
