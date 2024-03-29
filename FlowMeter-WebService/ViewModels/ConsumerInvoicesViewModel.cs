using Application.Models;

namespace FlowMeter_WebService.ViewModels
{
    public class ConsumerInvoicesViewModel
    {
        public Consumer Consumer { get; set; }
        public IEnumerable<Receipt> Receipts { get; set; }
    }
}