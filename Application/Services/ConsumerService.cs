namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class ConsumerService : IConsumerService
    {
        private readonly ILogger<ConsumerService> _logger;
        private readonly IConsumerRepository _consumerRepository;
        private readonly IHouseService _houseService;

        public ConsumerService(IConsumerRepository consumerRepository, IHouseService houseService, ILogger<ConsumerService> logger)
        {
            _consumerRepository = consumerRepository;
            _houseService = houseService;
            _logger = logger;
        }

        public async Task<Consumer> AddConsumer(Consumer consumer)
        {
            _logger.LogInformation("Adding a new consumer with Petsonal Account {ConsumerId}.", consumer.PersonalAccount);
            return await _consumerRepository.Add(consumer);
        }

        public async Task<List<Consumer>> GetList()
        {
            try
            {
                var all = await _consumerRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} consumers from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving consumers from the database.");
                throw;
            }
        }

        public async Task<List<SelectHouseDTO>> GetHouseOptions() 
        {
            return await _houseService.GetHouseOptions();
        }
    }
}
