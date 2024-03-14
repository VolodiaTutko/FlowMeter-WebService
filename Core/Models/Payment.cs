using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
	public class Payment
	{
		[Key]
		public int PaymentID { get; set; }
		[Required]
		public double Amount { get; set; }
		[Required]
		public DateTime Date { get; set; }
		[Required]
		public string PersonalAccount { get; set; }

		[Required]
		public string Type { get; set; }

		[ForeignKey(nameof(PersonalAccount))]
		public Consumer Consumer { get; set; }
	}
}
