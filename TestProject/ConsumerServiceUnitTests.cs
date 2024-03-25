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
        public async Task AddConsumer_InvalidInput_ThrowsException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ConsumerService>>();
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var consumerService = new ConsumerService(mockConsumerRepository.Object, mockHouseService.Object, mockLogger.Object);
            var consumer = new Consumer { PersonalAccount = "TestAccount" };

            mockConsumerRepository.Setup(repo => repo.Add(It.IsAny<Consumer>())).ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => consumerService.AddConsumer(consumer));
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

        [Fact]
        public async Task DeleteConsumer_ShouldDeleteConsumerFromRepository()
        {
            // Arrange
            var consumerId = "123";
            var mockRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var mockLogger = new Mock<ILogger<ConsumerService>>();

            var service = new ConsumerService(mockRepository.Object, mockHouseService.Object, mockLogger.Object);

            var consumerToDelete = new Consumer { PersonalAccount = consumerId };
            mockRepository.Setup(repo => repo.Delete(consumerId))
                          .ReturnsAsync(consumerToDelete);

            // Act
            var result = await service.DeleteConsumer(consumerId);

            // Assert
            mockRepository.Verify(repo => repo.Delete(consumerId), Times.Once);
            Assert.Equal(consumerToDelete, result);
        }

        [Fact]
        public async Task DeleteConsumer_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var consumerId = "123";
            var mockRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var mockLogger = new Mock<ILogger<ConsumerService>>();

            var service = new ConsumerService(mockRepository.Object, mockHouseService.Object, mockLogger.Object);
            mockRepository.Setup(repo => repo.Delete(consumerId))
                          .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.DeleteConsumer(consumerId));
        }

        [Fact]
        public async Task UpdateConsumer_ValidInput_ReturnsUpdatedConsumer()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ConsumerService>>();
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var consumerService = new ConsumerService(mockConsumerRepository.Object, mockHouseService.Object, mockLogger.Object);
            var updatedConsumer = new Consumer { PersonalAccount = "TestAccount", ConsumerOwner = "New Owner" };

            mockConsumerRepository.Setup(repo => repo.Update(It.IsAny<Consumer>())).ReturnsAsync(updatedConsumer);
            mockConsumerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Consumer());

            // Act
            var result = await consumerService.UpdateConsumer(updatedConsumer);

            // Assert
            Assert.Equal(updatedConsumer, result);
        }

        [Fact]
        public async Task UpdateConsumer_InvalidInput_ThrowsException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ConsumerService>>();
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var consumerService = new ConsumerService(mockConsumerRepository.Object, mockHouseService.Object, mockLogger.Object);
            var updatedConsumer = new Consumer { PersonalAccount = "TestAccount", ConsumerOwner = "New Owner" };

            mockConsumerRepository.Setup(repo => repo.Update(It.IsAny<Consumer>())).ThrowsAsync(new Exception("Simulated exception"));
            mockConsumerRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Consumer());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => consumerService.UpdateConsumer(updatedConsumer));
        }

        [Fact]
        public async Task UpdateConsumer_ConsumerFound_ReturnsUpdatedConsumer()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ConsumerService>>();
            var mockConsumerRepository = new Mock<IConsumerRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var consumerService = new ConsumerService(mockConsumerRepository.Object, mockHouseService.Object, mockLogger.Object);
            var existingConsumer = new Consumer { PersonalAccount = "ExistingAccount", ConsumerOwner = "Old Owner" };
            var updatedConsumer = new Consumer { PersonalAccount = "ExistingAccount", ConsumerOwner = "New Owner" };

            mockConsumerRepository.Setup(repo => repo.GetByIdAsync(existingConsumer.PersonalAccount)).ReturnsAsync(existingConsumer);
            mockConsumerRepository.Setup(repo => repo.Update(It.IsAny<Consumer>())).ReturnsAsync(updatedConsumer);

            // Act
            var result = await consumerService.UpdateConsumer(updatedConsumer);

            // Assert
            Assert.Equal(updatedConsumer, result);
        }
    }
}
