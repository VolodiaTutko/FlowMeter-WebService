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
        private readonly IUserRepository _userRepository;
        private readonly IHouseService _houseService;

        public ConsumerService(IConsumerRepository consumerRepository, IHouseService houseService, IUserRepository userRepository,  ILogger<ConsumerService> logger)
        {
            _consumerRepository = consumerRepository;
            _houseService = houseService;
            _logger = logger;
            _userRepository = userRepository;
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

        public async Task<Consumer> CreateConsumer(Consumer model, string houseAddress)
        {
            try
            {
                var houseModel = await _houseService.GetHouseByAddress(houseAddress);
                model.HouseId = houseModel.HouseId;

                var consumer = new Consumer
                {
                    PersonalAccount = model.PersonalAccount,
                    Flat = model.Flat,
                    ConsumerOwner = model.ConsumerOwner,
                    HeatingArea = model.HeatingArea,
                    HouseId = model.HouseId,
                    NumberOfPersons = model.NumberOfPersons,
                    ConsumerEmail = model.ConsumerEmail
                };

                await AddConsumer(consumer);
                _logger.LogInformation("Consumer created successfully with PersonalAccount: {ConsumerId}", consumer.PersonalAccount);

                return consumer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating consumer");
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
                var existingConsumer = await _consumerRepository.GetByIdAsync(consumer.PersonalAccount);
                if (existingConsumer == null)
                {
                    throw new ArgumentException("Consumer not found");
                }

                if (existingConsumer.ConsumerEmail != consumer.ConsumerEmail)
                {
                    // Check if the email is already associated with another consumer
                    var consumerWithEmailExists = await _userRepository.GetByEmailAsync(consumer.ConsumerEmail);
                    if (consumerWithEmailExists != null)
                    {
                        _logger.LogInformation("User with this email already exists.");
                        return existingConsumer;
                    }
                }

                // Update consumer details
                existingConsumer.ConsumerOwner = consumer.ConsumerOwner;
                existingConsumer.NumberOfPersons = consumer.NumberOfPersons;
                existingConsumer.ConsumerEmail = consumer.ConsumerEmail;

                var updatedConsumer = await _consumerRepository.Update(existingConsumer);
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

            var addedConsumer = await _consumerRepository.Delete(id);
            return addedConsumer;
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
