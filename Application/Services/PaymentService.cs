namespace Application.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConsumerService _consumerService;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository, IConsumerService consumerService, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _consumerService = consumerService;
            _logger = logger;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            try
            {
                var addedPayment = await _paymentRepository.Add(payment);
                _logger.LogInformation("Added a new payment to the database with ID: {PaymentID}", addedPayment.PaymentID);
                return addedPayment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a house to the database.");
                throw;
            }
        }

        public async Task<List<Payment>> GetList()
        {
            try
            {
                var all = await _paymentRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} payments from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving consumers from the database.");
                throw;
            }
        }

        public async Task<List<SelectConsumerDTO>> GetConsumerOptions()
        {
            return await _consumerService.GetConsumerOptions();
        }
    }
}
