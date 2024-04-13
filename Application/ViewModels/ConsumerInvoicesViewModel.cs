using Application.Models;

namespace Application.ViewModels
{
    public class ConsumerInvoicesViewModel
    {
        public Consumer Consumer { get; set; }

        public IEnumerable<Receipt> Receipts { get; set; }
        public Dictionary<string, int?> ServiceType { get; set; }
        public Dictionary<string, decimal?> Meters { get; set; }
    }
}