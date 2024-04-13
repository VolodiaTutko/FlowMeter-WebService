using Application.DataAccess;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                var addedUser = await _userRepository.Add(user);
                _logger.LogInformation("Adding a new service with User {UserId}.", addedUser.Id);
                return addedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a User to the database.");
                throw;
            }
        }

        public async Task<List<User>> GetList()
        {
            try
            {
                var all = await _userRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} services from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving services from the database.");
                throw;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var updatedUser = await _userRepository.Update(user);
                _logger.LogInformation("User with PersonalAccount: {PersonalAccount} updated successfully", updatedUser.Id);
                return updatedUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a user in the database.");
                throw;
            }
        }
    }
}
