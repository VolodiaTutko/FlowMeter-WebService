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
        public async Task AddAccount_SuccessfullyAddsAccount()
        {
            // Arrange
            var mockRepository = new Mock<IAccountRepository>();
            var accountToAdd = new Account { AccountID = 1, PersonalAccount = "TestAccount" };
            var addedAccount = new Account { AccountID = 1, PersonalAccount = "TestAccount" };

            mockRepository.Setup(repo => repo.Add(accountToAdd)).ReturnsAsync(addedAccount);
            var service = new AccountService(mockRepository.Object);

            // Act
            var result = await service.AddAccount(accountToAdd);

            // Assert
            Assert.Equal(addedAccount, result);
        }

        [Fact]
        public async Task AddAccount_ThrowsExceptionOnFailure()
        {
            // Arrange
            var mockRepository = new Mock<IAccountRepository>();
            var accountToAdd = new Account { AccountID = 1, PersonalAccount = "TestAccount" };

            // Simulate repository throwing exception
            mockRepository.Setup(repo => repo.Add(accountToAdd)).ThrowsAsync(new Exception("Failed to add account"));
            var service = new AccountService(mockRepository.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.AddAccount(accountToAdd));
        }
    }
}
