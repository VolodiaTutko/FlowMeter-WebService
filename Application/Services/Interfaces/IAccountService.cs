using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Account> AddAccount(Account account);

        Task<Account> GetAccountByPerconalAccount(string id);

        Task<Account> CreateAccount(Account account);

        Task SetConsumersCounter(Account account, string counterType, string accountcode);

        bool CheckIfCounterConnected(Account account, ServiceType counterType);

        bool CheckIfCounterConnected(Account account, string counterType);
    }
}
