using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
	public class Account
	{
		[Key]
		public int AccountID { get; set; }	
		[StringLength(10)]
		public string PersonalAccount { get; set; }
		[StringLength(10)]
		public string? HotWater { get; set; }
		[StringLength(10)]
		public string? ColdWater { get; set; }
		[StringLength(10)]
		public string? Heating { get; set; }
		[StringLength(10)]
		public string? Electricity { get; set; }
		[StringLength(10)]
		public string? Gas { get; set; }
		[StringLength(10)]
		public string? PublicService { get; set; }

		[ForeignKey(nameof(PersonalAccount))]
		public Consumer Consumer { get; set; }

	}
}
