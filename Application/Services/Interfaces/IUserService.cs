using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> AddUser(User user);

        public Task<List<User>> GetList();

        Task<User> GetUserByEmail(string email);

        Task<User> UpdateUser(User user);


    }
}
