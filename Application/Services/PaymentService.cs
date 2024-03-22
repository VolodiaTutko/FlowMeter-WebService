namespace Application.Services
{
    using Application.DataAccess;
    using Application.Models;
    using Application.Services.Interfaces;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Application.DTOS;

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConsumerService _consumerService;

        public PaymentService(IPaymentRepository paymentRepository, IConsumerService consumerService)
        {
            _paymentRepository = paymentRepository;
            _consumerService = consumerService;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            return await _paymentRepository.Add(payment);
        }

        public async Task<List<Payment>> GetList()
        {
            var all = await _paymentRepository.All();
            return all.Where(item => item != null).ToList();
        }
        public async Task<List<SelectConsumerDTO>> GetConsumerOptions()
        {
            
            return await _consumerService.GetConsumerOptions();
        }
    }
}
