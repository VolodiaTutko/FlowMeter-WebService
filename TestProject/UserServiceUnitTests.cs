using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DataAccess;
using Application.DTOS;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject
{
    public class UserServiceUnitTests
    {
        [Fact]
        public async Task AddUser_ValidUser_ReturnsAddedUser()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var userToAdd = new User {};
            var expectedUser = new User {};

            mockUserRepository.Setup(repo => repo.Add(userToAdd)).ReturnsAsync(expectedUser);

            // Act
            var result = await userService.AddUser(userToAdd);

            // Assert
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task AddUser_InvalidUser_ThrowsException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var userToAdd = new User {};

            mockUserRepository.Setup(repo => repo.Add(userToAdd)).ThrowsAsync(new Exception("Simulated error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await userService.AddUser(userToAdd));
        }

        [Fact]
        public async Task GetList_ReturnsListOfUsers()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var users = new List<User> {};

            mockUserRepository.Setup(repo => repo.All()).ReturnsAsync(users);

            // Act
            var result = await userService.GetList();

            // Assert
            Assert.Equal(users.Count, result.Count);
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetUserByEmail_ValidEmail_ReturnsUser()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var userEmail = "test@example.com";
            var expectedUser = new User {};

            mockUserRepository.Setup(repo => repo.GetByEmailAsync(userEmail)).ReturnsAsync(expectedUser);

            // Act
            var result = await userService.GetUserByEmail(userEmail);

            // Assert
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUserByEmail_InvalidEmail_ReturnsNull()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var userEmail = "invalid@example.com";

            mockUserRepository.Setup(repo => repo.GetByEmailAsync(userEmail)).ReturnsAsync((User)null);

            // Act
            var result = await userService.GetUserByEmail(userEmail);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUser_ValidUser_ReturnsUpdatedUser()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var userToUpdate = new User {};
            var expectedUpdatedUser = new User {};

            mockUserRepository.Setup(repo => repo.Update(userToUpdate)).ReturnsAsync(expectedUpdatedUser);

            // Act
            var result = await userService.UpdateUser(userToUpdate);

            // Assert
            Assert.Equal(expectedUpdatedUser, result);
        }

        [Fact]
        public async Task UpdateUser_InvalidUser_ThrowsException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockLogger = new Mock<ILogger<UserService>>();
            var userService = new UserService(mockUserRepository.Object, mockLogger.Object);
            var userToUpdate = new User {};

            mockUserRepository.Setup(repo => repo.Update(userToUpdate)).ThrowsAsync(new Exception("Simulated error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await userService.UpdateUser(userToUpdate));
        }
    }
}
