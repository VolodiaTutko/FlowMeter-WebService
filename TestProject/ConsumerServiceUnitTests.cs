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

    public class ConsumerServiceTests
    {
        [Fact]
        public async Task AddConsumer_ValidInput_ReturnsConsumer()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ConsumerService>>();
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var consumerService = new ConsumerService(mockConsumerRepository.Object, mockHouseService.Object, mockLogger.Object);
            var consumer = new Consumer { PersonalAccount = "TestAccount" };

            mockConsumerRepository.Setup(repo => repo.Add(It.IsAny<Consumer>())).ReturnsAsync(consumer);

            // Act
            var result = await consumerService.AddConsumer(consumer);

            // Assert
            Assert.Equal(consumer, result);
        }

        [Fact]
        public async Task GetList_WithValidData_ReturnsFilteredList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ConsumerService>>();
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var consumerService = new ConsumerService(mockConsumerRepository.Object, mockHouseService.Object, mockLogger.Object);
            var consumers = new List<Consumer>
            {
                new Consumer { PersonalAccount = "Account1" },
                new Consumer { PersonalAccount = "Account2" },
                null
            };

            mockConsumerRepository.Setup(repo => repo.All()).ReturnsAsync(consumers);

            // Act
            var result = await consumerService.GetList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.NotNull(item));
        }

        [Fact]
        public async Task GetConsumerOptions_ReturnsListOfConsumerOptions()
        {
            // Arrange
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var consumers = new List<Consumer>
            {
                new Consumer { PersonalAccount = "Account1" },
                new Consumer { PersonalAccount = "Account2" }
            };

            mockConsumerRepository.Setup(repo => repo.All()).ReturnsAsync(consumers);
            var consumerService = new ConsumerService(mockConsumerRepository.Object, null, null);

            // Act
            var result = await consumerService.GetConsumerOptions();

            // Assert
            Assert.Equal(consumers.Count, result.Count);
            Assert.All(result, item => Assert.NotNull(item));
        }
    }
}
