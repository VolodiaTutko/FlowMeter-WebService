﻿namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Application.ViewModels;

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
            var addedConsumer = await _consumerRepository.Add(consumer);
            _logger.LogInformation("Added a new consumer to the database with PersonalAccount: {PersonalAccount}", addedConsumer.PersonalAccount);
            return addedConsumer;
        }

        public async Task<Consumer> CreateConsumer(Consumer model, string houseAddress)
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

        public async Task<Consumer> GetConsumerByPersonalAccount(string personalAccount)
        {
            return await _consumerRepository.GetByIdAsync(personalAccount);
        }

        public async Task<Consumer> GetConsumerByEmail(string consumerEmail)
        {
            return await _consumerRepository.GetByEmailAsync(consumerEmail);
        }

        public async Task<List<Consumer>> GetList()
        {
            var all = await _consumerRepository.All();
            var filteredList = all.Where(item => item != null).ToList();
            _logger.LogInformation("Retrieved {Count} consumers from the database.", filteredList.Count);
            return filteredList;
        }

        public async Task<Consumer> UpdateConsumer(ConsumerUpdateViewModel consumer)
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
