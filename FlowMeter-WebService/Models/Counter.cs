using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.Models
{
	public class Counter
	{
		[Key]
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
