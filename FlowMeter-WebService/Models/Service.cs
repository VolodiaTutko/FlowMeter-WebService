using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowMeter_WebService.Models
{
	public class Service
	{
		[Key]
		public int ServiceId { get; set; }

		public int? HouseId { get; set; }
		[Required]
		public string TypeOfAccount { get; set; }

		public int? Price { get; set; }

		[ForeignKey(nameof(HouseId))]
		public House House { get; set; }
	
		
	}
}
