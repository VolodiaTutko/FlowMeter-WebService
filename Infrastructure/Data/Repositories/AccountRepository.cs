using Application.DataAccess;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Account> dbSet;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Account>();
        }

        public async Task<Account> Add(Account account)
        {
            _context.accounts.Add(account);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> GetAccountsByPersonal(string personal)
        {
            var account = await dbSet.FirstOrDefaultAsync(x => x.PersonalAccount == personal);
            return account;
        }
    }
}
