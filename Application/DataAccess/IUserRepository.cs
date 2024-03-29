using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.DataAccess
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);

        Task<List<User>> All();

        Task<User> Add(User user);

        Task<User> Update(User user);

        Task<User> Delete(int id);
        Task<User> GetByEmailAsync(string email);
    }

}