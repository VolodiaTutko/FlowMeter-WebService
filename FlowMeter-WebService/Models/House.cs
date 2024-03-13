using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.Models
{
	public class House
	{
		[Key]
		public int HouseId { get; set; }
		[MaxLength(80)]
		public string HouseAddress { get; set; }

		public int? HeatingAreaOfHouse { get; set; }

		public int? NumberOfFlat { get; set; }

		public int? NumberOfResidents { get; set; }
	}
}
