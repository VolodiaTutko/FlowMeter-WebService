using Application.Models;

namespace FlowMeter_WebService.ViewModels
{
    public class ConsumerInvoicesViewModel
    {
        public Consumer Consumer { get; set; }

        public IEnumerable<Receipt> Receipts { get; set; }
        public Dictionary<string, int?> ServiceType { get; set; }
        public Dictionary<string, decimal?> Meters { get; set; }
    }
}