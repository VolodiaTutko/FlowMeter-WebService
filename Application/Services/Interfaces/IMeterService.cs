namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IMeterService
    {
        public Task<Meter> AddMeter(Meter meter);

        public Task<MeterRecord> AddMeterRecord(MeterRecord record);

        public Task<Meter> UpdateMeter(Meter meter);

        public Task<Meter> DeleteMeter(int id);

        public Task<MeterInfoDTO> GetMeterById(int id);

        public Task<List<MeterInfoDTO>> GetList();
    }
}
