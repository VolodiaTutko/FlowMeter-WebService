using Application.DataAccess;
using Application.Models;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject
{
    public class ServiceServiceTests
    {
        [Fact]
        public async Task AddService_ValidInput_ReturnsAddedService()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ServiceService>>();
            var mockServiceRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var serviceService = new ServiceService(mockServiceRepository.Object, mockHouseService.Object, mockLogger.Object);
            var service = new Service { ServiceId = 1, TypeOfAccount = ServiceType.ColdWater.ToString() };
            mockServiceRepository.Setup(repo => repo.Add(It.IsAny<Service>())).ReturnsAsync(service);

            // Act
            var result = await serviceService.AddService(service);

            // Assert
            Assert.Equal(service, result);
        }

        [Fact]
        public async Task AddService_InvalidInput_ThrowsException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ServiceService>>();
            var mockServiceRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var serviceService = new ServiceService(mockServiceRepository.Object, mockHouseService.Object, mockLogger.Object);
            var service = new Service { ServiceId = 1, TypeOfAccount = ServiceType.ColdWater.ToString() };
            mockServiceRepository.Setup(repo => repo.Add(It.IsAny<Service>())).ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => serviceService.AddService(service));
        }

        [Fact]
        public async Task GetList_WithValidData_ReturnsFilteredList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ServiceService>>();
            var mockServiceRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var serviceService = new ServiceService(mockServiceRepository.Object, mockHouseService.Object, mockLogger.Object);
            var services = new List<Service>
            {
                new Service { ServiceId = 1, TypeOfAccount = ServiceType.ColdWater.ToString()},
                new Service { ServiceId = 2, TypeOfAccount = ServiceType.HotWater.ToString()},
                null
            };

            mockServiceRepository.Setup(repo => repo.All()).ReturnsAsync(services);

            // Act
            var result = await serviceService.GetList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.NotNull(item));
        }

        [Fact]
        public async Task DeleteService_ShouldDeleteServiceFromRepository()
        {
            // Arrange
            var serviceId = 123;
            var mockRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var mockLogger = new Mock<ILogger<ServiceService>>();

            var serviceService = new ServiceService(mockRepository.Object, mockHouseService.Object, mockLogger.Object);
            var serviceToDelete = new Service { ServiceId = 1, TypeOfAccount = ServiceType.ColdWater.ToString() };
            mockRepository.Setup(repo => repo.Delete(serviceId)).ReturnsAsync(serviceToDelete);

            // Act
            var result = await serviceService.DeleteService(serviceId);

            // Assert
            mockRepository.Verify(repo => repo.Delete(serviceId), Times.Once);
            Assert.Equal(serviceToDelete, result);
        }

        [Fact]
        public async Task DeleteService_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var serviceId = 123;
            var mockRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var mockLogger = new Mock<ILogger<ServiceService>>();

            var serviceService = new ServiceService(mockRepository.Object, mockHouseService.Object, mockLogger.Object);
            mockRepository.Setup(repo => repo.Delete(serviceId)).ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => serviceService.DeleteService(serviceId));
        }

        [Fact]
        public async Task UpdateService_ValidInput_ReturnsUpdatedService()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ServiceService>>();
            var mockServiceRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var serviceService = new ServiceService(mockServiceRepository.Object, mockHouseService.Object, mockLogger.Object);
            var updatedService = new Service {ServiceId = 1, TypeOfAccount = ServiceType.ColdWater.ToString() };

            mockServiceRepository.Setup(repo => repo.Update(It.IsAny<Service>())).ReturnsAsync(updatedService);

            // Act
            var result = await serviceService.UpdateService(updatedService);

            // Assert
            Assert.Equal(updatedService, result);
        }

        [Fact]
        public async Task UpdateService_InvalidInput_ThrowsException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ServiceService>>();
            var mockServiceRepository = new Mock<IServiceRepository>();
            var mockHouseService = new Mock<IHouseService>();
            var serviceService = new ServiceService(mockServiceRepository.Object, mockHouseService.Object, mockLogger.Object);
            var updatedService = new Service {ServiceId = 1, TypeOfAccount = ServiceType.ColdWater.ToString() };

            mockServiceRepository.Setup(repo => repo.Update(It.IsAny<Service>())).ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => serviceService.UpdateService(updatedService));
        }
    }
}
