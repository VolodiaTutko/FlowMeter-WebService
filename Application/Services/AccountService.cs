using Application.DataAccess;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<Account> AddAccount(Account account)
        {
            var addedAccount = await _accountRepository.Add(account);
            return addedAccount;
        }

        public async Task<Account> CreateAccount(Account model)
        {
            try
            {
                var account = new Account
                {
                    PersonalAccount = model.PersonalAccount,
                    HotWater = null,
                    ColdWater = null,
                    Heating = null,
                    Electricity = null,
                    Gas = null,
                    PublicService = null
                };


                await AddAccount(account);
                _logger.LogInformation("Account created successfully for PersonalAccount: {ConsumerId}", account.PersonalAccount);

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating account");
                throw;
            }
        }
    }
}
