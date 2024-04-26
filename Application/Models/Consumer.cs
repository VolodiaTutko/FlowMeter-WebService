namespace Application.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Consumer
    {
        [Key]
        [MaxLength(10)]
        public string PersonalAccount { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Range(1, int.MaxValue, ErrorMessage = "Номер квартири має бути більшим за 0")]
        public int Flat { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [RegularExpression(@"^[A-ZА-Я][^\d]+\s+[A-ZА-Я][^\d]+$", ErrorMessage = "Прізвище записано невірно")]
        public string ConsumerOwner { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Range(1, int.MaxValue, ErrorMessage = "Площа має бути більшою за 0")]
        public int HeatingArea { get; set; }

        [Required]
        public int HouseId { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [Range(1, int.MaxValue, ErrorMessage = "Кількість мешканців має бути більша за 0")]
        public int NumberOfPersons { get; set; }

        public string ConsumerEmail { get; set; }

        [ForeignKey(nameof(HouseId))]
        public House House { get; set; }
    }
}
