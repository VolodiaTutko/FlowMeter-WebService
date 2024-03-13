using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowMeter_WebService.Models
{
	public class Accrual
	{
		[Key]
		public int AccrualID { get; set; }

		[Required]
		public string PersonalAccount { get; set; }


		[Required]
		public double Accrued { get; set; }

		public double? PreviuosDebit { get; set; }

		[Required]
		public decimal? Paid; 

		[Required]
		public DateTime Date {  get; set; }

		[ForeignKey(nameof(PersonalAccount))]
		public Consumer Consumer { get; set; }
	}
}
