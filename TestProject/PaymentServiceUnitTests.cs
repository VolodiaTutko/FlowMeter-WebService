namespace TestProject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    
    public class PaymentServiceTests
    {
        [Fact]
        public async Task AddPayment_ShouldReturnAddedPayment()
        {
            // Arrange
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var mockConsumerService = new Mock<IConsumerService>();
            var paymentService = new PaymentService(mockPaymentRepository.Object, mockConsumerService.Object);
            var payment = new Payment { PaymentID = 1 };

            mockPaymentRepository.Setup(repo => repo.Add(payment)).ReturnsAsync(payment);

            // Act
            var result = await paymentService.AddPayment(payment);

            // Assert
            Assert.Equal(payment, result);
        }

        [Fact]
        public async Task GetList_ShouldReturnListOfPayments()
        {
            // Arrange
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var mockConsumerService = new Mock<IConsumerService>();
            var paymentService = new PaymentService(mockPaymentRepository.Object, mockConsumerService.Object);
            var payments = new List<Payment> 
            {
                new Payment { PaymentID = 1, Amount = 100 },
                new Payment { PaymentID = 2, Amount = 200 }
            };

            mockPaymentRepository.Setup(repo => repo.All()).ReturnsAsync(payments);

            // Act
            var result = await paymentService.GetList();

            // Assert
            Assert.Equal(payments, result);
        }

        [Fact]
        public async Task GetConsumerOptions_ShouldReturnListOfConsumerOptions()
        {
            // Arrange
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var mockConsumerService = new Mock<IConsumerService>();


            var paymentService = new PaymentService(mockPaymentRepository.Object, mockConsumerService.Object);
            var consumerOptions = new List<SelectConsumerDTO> { };

            mockConsumerService.Setup(service => service.GetConsumerOptions()).ReturnsAsync(consumerOptions);

            // Act
            var result = await paymentService.GetConsumerOptions();

            // Assert
            Assert.Equal(consumerOptions, result);
        }
    }
    
}
