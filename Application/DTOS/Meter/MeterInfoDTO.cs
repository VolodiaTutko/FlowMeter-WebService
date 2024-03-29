
namespace Application.DTOS
{
    using Application.Models;

    public class MeterInfoDTO
    {
        public MeterInfoDTO(Meter meter, List<MeterRecord> records)
        {
            this.MeterId = meter.MeterId;
            this.CounterAccount = meter.CounterAccount;
            this.TypeOfAccount = meter.TypeOfAccount;
            this.Role = meter.Role;
            var lastRecord = records.OrderByDescending(x => x.Date)
                .FirstOrDefault();
            if (lastRecord != null)
            {
                this.CurrentIndicator = lastRecord.CurrentIndicator;
                this.LastModified = lastRecord.Date;
            } else
            {
                this.CurrentIndicator = null;
                this.LastModified = null;
            }
        }

        public int MeterId { get; set; }

        public string CounterAccount { get; set; }

        public string TypeOfAccount { get; set; }

        public string Role { get; set; }

        public decimal? CurrentIndicator { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
