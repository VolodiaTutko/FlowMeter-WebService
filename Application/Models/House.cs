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

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [MaxLength(80)]
        [RegularExpression(@"^вул\.\s+[^\d]+\s+\d+$", ErrorMessage = "Неправильний формат адреси. Приклад вірного формату: вул. Прикладна 123")]
        public string HouseAddress { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Площа має бути більшою за 0")]
        public int HeatingAreaOfHouse { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість квартир має бути більшою за 0")]
        public int NumberOfFlat { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Кількість осіб має бути більшою за 0")]
        public int NumberOfResidents { get; set; }
    }
}
