namespace Application.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class House
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HouseId { get; set; }

        [MaxLength(80)]
        public string HouseAddress { get; set; }

        public int? HeatingAreaOfHouse { get; set; }

        public int? NumberOfFlat { get; set; }

        public int? NumberOfResidents { get; set; }

        
    }
}
