using System.ComponentModel.DataAnnotations;

namespace FlowMeter_WebService.Models
{
	public class Admin
	{
		[Key] 
		public string AdminEmail { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
