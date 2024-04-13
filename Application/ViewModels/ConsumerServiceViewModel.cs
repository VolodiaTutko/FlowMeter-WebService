using Application.Models;

namespace Application.ViewModels
{
    public class ConsumerServiceViewModel
    {
        //public Consumer Consumer { get; set; }
        public Service ServiceType { get; set; }
        //public int? CostPerUnit { get; set; }
        public Meter Meters { get; set; }
    }
}
