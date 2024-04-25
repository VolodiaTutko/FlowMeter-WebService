namespace Application.DTOS
{
    public class NewMeterRecordVm
    {
        public decimal CurrentIndicator { get; set; }

        public DateTime Date { get; set; }

        public int MeterId { get; set; }

        public NewMeterRecordVm(decimal indicator, DateTime date, int meterId)
        {
            this.CurrentIndicator = indicator;
            this.Date = date;
            this.MeterId = meterId;
        }
    }
}
