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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<User> dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<User>();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> All()
        {
            var user = await dbSet.ToListAsync();
            return user;
        }

        public async Task<User> Add(User user)
        {
            try
            {
                _context.users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                // Обробка помилки
                throw new Exception("Помилка при додаванні сервісу до бази даних.", ex);
            }
        }

        public Task<User> Update(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.users.FirstOrDefaultAsync(c => c.ConsumerEmail == email);

        }
    }
}
