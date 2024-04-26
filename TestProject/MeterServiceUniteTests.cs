using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DTOS;
using Application.Models;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProject
{
    public class MeterServiceUnitTests
    {
        private readonly Mock<IMeterRepository> mockMeterRepository;
        private readonly Mock<ILogger<MeterService>> mockLogger;
        private readonly Mock<IConsumerRepository> mockConsumerRepository;
        private readonly Mock<IMeterRecordRepository> mockMeterRecordRepository;
        private readonly Mock<IAccountRepository> mockAccountRepository;
        private readonly Mock<IAccountService> mockAccountService;
        private readonly Mock<IServiceRepository> mockServiceRepository;

        private readonly MeterService meterService;

        public MeterServiceUnitTests()
        {
            mockMeterRepository = new Mock<IMeterRepository>();
            mockLogger = new Mock<ILogger<MeterService>>();
            mockConsumerRepository = new Mock<IConsumerRepository>();
            mockMeterRecordRepository = new Mock<IMeterRecordRepository>();
            mockAccountRepository = new Mock<IAccountRepository>();
            mockAccountService = new Mock<IAccountService>();
            mockServiceRepository = new Mock<IServiceRepository>();

            meterService = new MeterService(mockServiceRepository.Object, mockAccountService.Object,
                                             mockMeterRepository.Object, mockConsumerRepository.Object,
                                             mockMeterRecordRepository.Object, mockLogger.Object, mockAccountRepository.Object);
        }

        [Fact]
        public async Task GetMeterByCounterAccount_ShouldReturnMeter_WhenValidIdProvided()
        {
            // Arrange
            string counterAccountId = "12345";
            Meter expectedMeter = new Meter { MeterId = 1, CounterAccount = counterAccountId };

            mockMeterRepository.Setup(repo => repo.GetByCounterAccountAsync(counterAccountId))
                               .ReturnsAsync(expectedMeter);

            // Act
            Meter actualMeter = await meterService.GetMeterByCounterAccount(counterAccountId);

            // Assert
            actualMeter.Equals(expectedMeter);
        }


        [Fact]
        public async Task AddMeter_ShouldAddMeterAndLog_WhenValidMeterProvided()
        {
            // Arrange
            Meter meterToAdd = new Meter { MeterId = 0, CounterAccount = "12345" };
            Meter expectedAddedMeter = new Meter { MeterId = 1, CounterAccount = "12345" };

            mockMeterRepository.Setup(repo => repo.Add(meterToAdd))
                               .ReturnsAsync(expectedAddedMeter);

            // Act
            Meter actualAddedMeter = await meterService.AddMeter(meterToAdd);

            // Assert
            mockMeterRepository.Verify(repo => repo.Add(meterToAdd), Times.Once);
            Assert.Equal(expectedAddedMeter, actualAddedMeter);
        }

        [Fact]
        public async Task DeleteMeter_ShouldDeleteMeterAndLog_WhenValidIdProvided()
        {
            // Arrange
            int meterIdToDelete = 1;
            Meter expectedDeletedMeter = new Meter { MeterId = meterIdToDelete, CounterAccount = "12345" };

            mockMeterRepository.Setup(repo => repo.Delete(meterIdToDelete))
                               .ReturnsAsync(expectedDeletedMeter);

            // Act
            Meter actualDeletedMeter = await meterService.DeleteMeter(meterIdToDelete);

            // Assert
            mockMeterRepository.Verify(repo => repo.Delete(meterIdToDelete), Times.Once);
            Assert.Equal(expectedDeletedMeter, actualDeletedMeter);
        }


        [Fact]
        public async Task GetMeterByCounterAccount_ShouldReturnNull_WhenInvalidIdProvided()
        {
            // Arrange
            string counterAccountId = "InvalidId";
            mockMeterRepository.Setup(repo => repo.GetByCounterAccountAsync(counterAccountId))
                .ReturnsAsync((Meter)null);

            // Act
            Meter actualMeter = await meterService.GetMeterByCounterAccount(counterAccountId);

            // Assert
            Assert.Null(actualMeter);
        }

        [Fact]
        public async Task DeleteMeter_ShouldThrowException_WhenInvalidIdProvided()
        {
            // Arrange
            int meterIdToDelete = -1;
            mockMeterRepository.Setup(repo => repo.Delete(meterIdToDelete))
                .ThrowsAsync(new ArgumentException("Invalid Meter ID"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => meterService.DeleteMeter(meterIdToDelete));
        }
    }
}