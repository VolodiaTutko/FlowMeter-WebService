using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject
{
    public class MeterServiceUnitTests
    {
        [Fact]
        public async Task AddMeter_SuccessfullyAddsMeter()
        {
            // Arrange
            var mockMeterRepo = new Mock<IMeterRepository>();
            var mockMeterRecRepo = new Mock<IMeterRecordRepository>();
            var mockLogger = new Mock<ILogger<MeterService>>();

            var meterToAdd = new Meter { MeterId = 1, CounterAccount = "TestAccount" };
            var addedMeter = new Meter { MeterId = 1, CounterAccount = "TestAccount" };

            mockMeterRepo.Setup(repo => repo.Add(It.IsAny<Meter>())).ReturnsAsync(addedMeter);
            var service = new MeterService(mockMeterRepo.Object, mockMeterRecRepo.Object, mockLogger.Object);

            // Act
            var result = await service.AddMeter(meterToAdd);

            // Assert
            Assert.Equal(addedMeter, result);
        }

        [Fact]
        public async Task UpdateMeter_SuccessfullyUpdatesMeter()
        {
            // Arrange
            var mockMeterRepo = new Mock<IMeterRepository>();
            var mockMeterRecRepo = new Mock<IMeterRecordRepository>();
            var mockLogger = new Mock<ILogger<MeterService>>();

            var meterToUpdate = new Meter { MeterId = 1, CounterAccount = "TestAccount" };
            var updatedMeter = new Meter { MeterId = 1, CounterAccount = "UpdatedTestAccount" };

            mockMeterRepo.Setup(repo => repo.Update(It.IsAny<Meter>())).ReturnsAsync(updatedMeter);
            var service = new MeterService(mockMeterRepo.Object, mockMeterRecRepo.Object, mockLogger.Object);

            // Act
            var result = await service.UpdateMeter(meterToUpdate);

            // Assert
            Assert.Equal(updatedMeter, result);
        }

        [Fact]
        public async Task DeleteMeter_SuccessfullyDeletesMeter()
        {
            // Arrange
            var mockMeterRepo = new Mock<IMeterRepository>();
            var mockMeterRecRepo = new Mock<IMeterRecordRepository>();
            var mockLogger = new Mock<ILogger<MeterService>>();

            var meterToDelete = new Meter { MeterId = 1, CounterAccount = "TestAccount" };

            mockMeterRepo.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(meterToDelete);
            var service = new MeterService(mockMeterRepo.Object, mockMeterRecRepo.Object, mockLogger.Object);

            // Act
            var result = await service.DeleteMeter(1);

            // Assert
            Assert.Equal(meterToDelete, result);
        }
    }
}