﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowMeter_WebService.Models
{
    public class Consumer
    {
        [Key]
        public int ConsumersId { get; set; }
        [Required]
		[MaxLength(10)]
		public string PersonalAccount { get; set; }
		[Required]
		public int Flat { get; set; }
		[Required]
		public string ConsumerOwner { get; set; }
		[Required]
		public int HeatingArea { get; set; }
		[Required]
		public int HouseId { get; set; }
		[Required]
		
		public int NumberOfPersons { get; set; }
		[Required]
		public string ConsumerEmail { get; set; }


		[ForeignKey(nameof(PersonalAccount))] 
		public Account Account { get; set; }

		[ForeignKey(nameof(HouseId))]
		public House House { get; set; }

		[ForeignKey(nameof(ConsumerEmail))]
		public User User { get; set; }
	}
}

