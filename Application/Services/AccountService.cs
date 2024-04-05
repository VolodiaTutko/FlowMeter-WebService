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
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> AddAccount(Account account)
        {
            var addedAccount = await _accountRepository.Add(account);
            return addedAccount;
        }

        public async Task<Account> GetAccountsByPersonal(string personalAccount)
        {
            return await this._accountRepository.GetAccountsByPersonal(personalAccount);
        }
    }
}
