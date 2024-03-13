using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.Models
{
	public class User
	{
		[Key]
		public string ConsumerEmail { get; set; }
		[Required]
		public string Password { get; set; }	
		
	}
}
