namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class MeterService : IMeterService
    {
        private readonly ILogger<MeterService> logger;
        private readonly IMeterRepository meterRepository;
        private readonly IMeterRecordRepository meterRecRepository;

        public MeterService(IMeterRepository meterRepo, IMeterRecordRepository meterRecRepo, ILogger<MeterService> logger)
        {
            this.meterRepository = meterRepo;
            this.meterRecRepository = meterRecRepo;
            this.logger = logger;
        }

        public async Task<Meter> GetMeterByCounterAccount(string id)
        {
            return await meterRepository.GetByCounterAccountAsync(id);
        }

        public async Task<Meter> AddMeter(Meter meter)
        {
            try
            {
                var addedMeter = await this.meterRepository.Add(meter);
                this.logger.LogInformation("Added a new meter to the database with ID: {MeterId} for account: {CounterAccount}", addedMeter.MeterId, addedMeter.CounterAccount);
                return addedMeter;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while adding a meter to the database.");
                throw;
            }
        }

        public async Task<MeterRecord> AddMeterRecord(MeterRecord record)
        {
            try
            {
                var addedRecord = await this.meterRecRepository.Add(record);
                this.logger.LogInformation("Added a new meter record to the database with ID: {MeterRecordId} for meter: {MeterId}", addedRecord.MeterRecordId, addedRecord.Meter.MeterId);
                return addedRecord;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while adding a meter record for meter: {MeterId} to the database.", record.Meter.MeterId);
                throw;
            }
        }

        public async Task<Meter> UpdateMeter(Meter meter)
        {
            try
            {
                var updatedMeter = await this.meterRepository.Update(meter);
                this.logger.LogInformation("Updated meter with ID: {MeterId} for account: {CounterAccount}", updatedMeter.MeterId, updatedMeter.CounterAccount);
                return updatedMeter;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while updating meter with ID: {MeterId} for account: {CounterAccount}", meter.MeterId, meter.CounterAccount);
                throw;
            }
        }

        public async Task<Meter> DeleteMeter(int id)
        {
            try
            {
                var deletedMeter = await this.meterRepository.Delete(id);
                this.logger.LogInformation("Deleted meter with ID: {MeterId} for account: {CounterAccount}", deletedMeter.MeterId, deletedMeter.CounterAccount);
                return deletedMeter;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while deleting meter with ID: {MeterId}", id);
                throw;
            }
        }

        public async Task<MeterInfoDTO> GetMeterById(int id)
        {
            try
            {
                var meter = await this.meterRepository.GetByIdAsync(id);
                var records = await this.meterRecRepository.GetListByMeterId(id);
                return new MeterInfoDTO(meter, records);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while fetching meter with ID: {id}", id);
                throw;
            }
        }

        public async Task<List<MeterInfoDTO>> GetList()
        {
            try
            {
                var meters = await this.meterRepository.All();
                var meterInfoList = new List<MeterInfoDTO>();
                foreach (var meter in meters)
                {
                    var records = await this.meterRecRepository.GetListByMeterId(meter.MeterId);
                    meterInfoList.Add(new MeterInfoDTO(meter, records));
                }

                return meterInfoList;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while fetching the list of meters.");
                throw;
            }
        }

        Task<Meter> IMeterService.GetMeterByCounterAccount(string id)
        {
            throw new NotImplementedException();
        }
    }
}
