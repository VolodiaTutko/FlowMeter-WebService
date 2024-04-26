namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IMeterService
    {
        public Task<Meter> AddMeter(Meter meter);

        public Task<List<LookUpUntrackedConsumersDto>> GetMeterAsignableAccounts();

        public Task<Meter> RegisterMeter(CreateMeterVm createMeterVm);

        public Task<MeterRecord> RegisterRecordAdmin(NewMeterRecordVm createMeterRecordVm);

        public Task<MeterRecord> RegisterRecordConsumer(NewMeterRecordVm createMeterRecordVm);

        public Task<Meter> UpdateMeter(Meter meter);

        public Task<Meter> DeleteMeter(int id);

        public Task<MeterInfoDTO> GetMeterById(int id);

        public Task<MeterInfoDTO> GetMeterInfoByAccount(string acc);

        public Task<List<MeterInfoDTO>> GetList();

        Task<Meter> GetMeterByCounterAccount(string id);
    }
}
