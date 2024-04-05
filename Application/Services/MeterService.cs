using Application.DataAccess;
using Application.DTOS;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Application.Services
{
    public class MeterService : IMeterService
    {
        private readonly ILogger<MeterService> _logger;
        private readonly IMeterRepository _meterRepository;

        public MeterService(IMeterRepository meterRepository, ILogger<MeterService> logger)
        {
            _meterRepository = meterRepository;
            _logger = logger;
        }

        public async Task<Meter> GetMeterByCounterAccount(string id)
        {
            return await _meterRepository.GetByCounterAccountAsync(id);
        }
    }
}
