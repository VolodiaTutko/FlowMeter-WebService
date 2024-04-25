namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.DTOS.Service;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;

    public class MeterService : IMeterService
    {
        private readonly ILogger<MeterService> logger;
        private readonly IMeterRepository meterRepository;
        private readonly IMeterRecordRepository meterRecRepository;
        private readonly IConsumerRepository consumerRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IAccountService accountService;
        private readonly IServiceRepository serviceRepository;

        public MeterService(IServiceRepository _serviceRepository, IAccountService _accService, IMeterRepository meterRepo, IConsumerRepository _consumerRepository, IMeterRecordRepository meterRecRepo, ILogger<MeterService> logger, IAccountRepository _accountRepository)
        {
            this.meterRepository = meterRepo;
            this.meterRecRepository = meterRecRepo;
            this.logger = logger;
            this.consumerRepository = _consumerRepository;
            accountRepository = _accountRepository;
            this.accountService = _accService;
            this.serviceRepository = _serviceRepository;
        }

        public async Task<Meter> RegisterMeter(CreateMeterVm createMeterVm)
        {
            // add meter: generate guid, assign it to the account column of the type, add new meter to the meters, assign account to meter
            // then, add first meter record

            var account = await accountRepository.GetByIdAsync(createMeterVm.OwnerId);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            if (accountService.CheckIfCounterConnected(account, createMeterVm.TypeOfAccount))
            {
                throw new Exception("Counter already connected");
            }

            var acc = GenerateRandomString(10);

            var meter = new Meter()
            {
                CounterAccount = acc,
                TypeOfAccount = createMeterVm.TypeOfAccount,
                Role = "flat"
            };

            var addedMeter = await AddMeter(meter);

            await accountService.SetConsumersCounter(account, createMeterVm.TypeOfAccount, acc);

            var record = new MeterRecord()
            {
                Meter = addedMeter,
                CurrentIndicator = createMeterVm.CurrentIndicator,
                Date = createMeterVm.Date.ToUniversalTime(),
                //Method = "initial admin"
            };

            var addedRecord = await AddMeterRecord(record, "initial admin");

            return addedMeter;
        }

        public async Task<MeterRecord> RegisterRecordAdmin(NewMeterRecordVm createMeterRecordVm)
        {
            var meter = await meterRepository.GetByIdAsync(createMeterRecordVm.MeterId);

            if (meter == null)
            {
                throw new Exception("Meter not found");
            }

            var record = new MeterRecord()
            {
                Meter = meter,
                CurrentIndicator = createMeterRecordVm.CurrentIndicator,
                Date = createMeterRecordVm.Date.ToUniversalTime(),
                //Method = "admin"
            };

            var addedRecord = await AddMeterRecord(record, "admin");

            return addedRecord;
        }

        public async Task<MeterRecord> RegisterRecordConsumer(NewMeterRecordVm createMeterRecordVm)
        {
            var meter = await meterRepository.GetByIdAsync(createMeterRecordVm.MeterId);

            if (meter == null)
            {
                throw new Exception("Meter not found");
            }

            var record = new MeterRecord()
            {
                Meter = meter,
                CurrentIndicator = createMeterRecordVm.CurrentIndicator,
                Date = createMeterRecordVm.Date.ToUniversalTime(),
                //Method = "consumer"
            };

            var addedRecord = await AddMeterRecord(record, "consumer");

            return addedRecord;
        }

        public async Task<List<LookUpUntrackedConsumersDto>> GetMeterAsignableAccounts()
        {
            // get all users
            // for each user, get service with no meter
            // for each account create a service dto
            // return composite

            var lookUp = new List<LookUpUntrackedConsumersDto> ();

            var consumers = await consumerRepository.All();
            foreach (var consumer in consumers)
            {
                var userServices = new List<LookUpUserServiceDto>();
                var account = await accountRepository.GetByIdAsync(consumer.PersonalAccount);
                var services = await serviceRepository.GetByHouseIdAsync(consumer.HouseId);
                foreach (var service in services)
                {
                    bool alreadyWithMeter = accountService.CheckIfCounterConnected(account, service.TypeOfAccount);
                    if (!alreadyWithMeter)
                    {
                        userServices.Add(new LookUpUserServiceDto(service.TypeOfAccount, service.TypeOfAccount));
                    }

                }

                if (userServices.Count > 0) { 
                    lookUp.Add(new LookUpUntrackedConsumersDto(consumer, userServices));
                }
            }

            return lookUp;
        }

        public async Task<Meter> GetMeterByCounterAccount(string id)
        {
            return await this.meterRepository.GetByCounterAccountAsync(id);
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

        private async Task<MeterRecord> AddMeterRecord(MeterRecord record, string method)
        {
            //record.Method = method;
            this.logger.LogInformation("Adding a new meter record for meter: {MeterId} to the database. Method: {method}", record.Meter.MeterId, method);
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

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] randomChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                randomChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomChars);
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

        public async Task<MeterInfoDTO> GetMeterInfoByAccount(string acc)
        {
            try
            {
                var meter = await this.meterRepository.GetByCounterAccountAsync(acc);

                var records = new List<MeterRecord>();

                if (meter != null)
                {
                    records = await this.meterRecRepository.GetListByMeterId(meter.MeterId);
                } else
                {
                    throw new Exception("Meter not found");
                }
                
                return new MeterInfoDTO(meter, records);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while fetching meter with CounterAccount: {acc}", acc);
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
    }
}
