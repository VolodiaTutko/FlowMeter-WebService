namespace Application.DTOS
{
    public class CreateMeterVm
    {
        public string TypeOfAccount { get; set; }

        public decimal CurrentIndicator { get; set; }

        public DateTime Date { get; set; }

        public string OwnerId { get; set; }

        public CreateMeterVm(string type, decimal indicator, DateTime date, string ownerId)
        {
            this.TypeOfAccount = type;
            this.CurrentIndicator = indicator;
            this.Date = date;
            this.OwnerId = ownerId;
        }
    }
}
