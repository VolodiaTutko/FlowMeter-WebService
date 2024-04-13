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
    public class AccountServiceUnitTests
    {
        [Fact]
        public async Task CreateAccount_SuccessfullyCreatesAccount()
        {
            // Arrange
            var mockRepository = new Mock<IAccountRepository>();
            var mockLogger = new Mock<ILogger<AccountService>>();
            var accountToAdd = new Account { PersonalAccount = "TestAccount" };
            var addedAccount = new Account { AccountID = 1, PersonalAccount = "TestAccount" };

            mockRepository.Setup(repo => repo.Add(It.IsAny<Account>())).ReturnsAsync(addedAccount);
            var service = new AccountService(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await service.AddAccount(accountToAdd);

            // Assert
            Assert.Equal(addedAccount, result);
        }

        [Fact]
        public async Task CreateAccount_ThrowsExceptionOnFailure()
        {
            // Arrange
            var mockRepository = new Mock<IAccountRepository>();
            var mockLogger = new Mock<ILogger<AccountService>>();
            var accountToAdd = new Account { PersonalAccount = "TestAccount" };

            // Simulate repository throwing exception
            mockRepository.Setup(repo => repo.Add(It.IsAny<Account>())).ThrowsAsync(new Exception("Failed to add account"));
            var service = new AccountService(mockRepository.Object, mockLogger.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task GetAccountByPersonalAccount_ReturnsCorrectAccount()
        {
            // Arrange
            var mockRepository = new Mock<IAccountRepository>();
            var mockLogger = new Mock<ILogger<AccountService>>();
            var expectedAccount = new Account { AccountID = 1, PersonalAccount = "TestAccount" };

            mockRepository.Setup(repo => repo.GetByIdAsync("TestAccount")).ReturnsAsync(expectedAccount);
            var service = new AccountService(mockRepository.Object, mockLogger.Object);

            // Act
            var result = await service.GetAccountByPerconalAccount("TestAccount");

            // Assert
            Assert.Equal(expectedAccount, result);
        }
    }
}
