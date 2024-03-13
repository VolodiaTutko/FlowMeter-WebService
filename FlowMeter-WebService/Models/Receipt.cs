using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowMeter_WebService.Models
{
	public class Receipt
	{
		[Key]
		public int ReceiptId { get; set; }
		[Required]
		public string PersonalAccount { get; set; }
		[Required]
		public DateTime Date { get; set; }

		public byte[]?  PDF { get; set; }	

		[ForeignKey(nameof(PersonalAccount))]
		public Consumer Consumer { get; set; }

	}
}
