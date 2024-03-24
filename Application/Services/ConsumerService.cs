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
            try
            {
                var addedConsumer = await _consumerRepository.Add(consumer);
                _logger.LogInformation("Added a new consumer to the database with PersonalAccount: {PersonalAccount}", addedConsumer.PersonalAccount);
                return addedConsumer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a consumer to the database.");
                throw;
            }
        }

        public async Task<Consumer> GetConsumerByPersonalAccount(string personalAccount)
        {
            return await _consumerRepository.GetByIdAsync(personalAccount);
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

        public async Task<Consumer> UpdateConsumer(Consumer consumer)
        {
            try
            {
                var updatedConsumer = await _consumerRepository.Update(consumer);
                _logger.LogInformation("Consumer with PersonalAccount: {PersonalAccount} updated successfully", updatedConsumer.PersonalAccount);
                return updatedConsumer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a consumer in the database.");
                throw;
            }
        }

        public async Task<Consumer> DeleteConsumer(string id)
        {
            try
            {
                var addedConsumer = await _consumerRepository.Delete(id);
                _logger.LogInformation("Consumer with PersonalAccount: {PersonalAccount} deleted successfully", addedConsumer.PersonalAccount);
                return addedConsumer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a consumer from the database.");
                throw;
            }
        }

        public async Task<List<SelectHouseDTO>> GetHouseOptions() 
        {
            return await _houseService.GetHouseOptions();
        }

        public async Task<List<SelectConsumerDTO>> GetConsumerOptions()
        {
            var allConsumer = await _consumerRepository.All();
            List<SelectConsumerDTO> options = new List<SelectConsumerDTO>();
            allConsumer.ForEach(item => options.Add(new SelectConsumerDTO(item)));
            return options;
        }
    }
}
