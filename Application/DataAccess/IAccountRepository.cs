using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataAccess
{
    public interface IAccountRepository
    {
        Task<Account> Add(Account account);
        Task<Account> GetByIdAsync(string id);
    }
}
