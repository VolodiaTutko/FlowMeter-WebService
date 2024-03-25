﻿namespace Infrastructure.Data.Repositories
{
    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class ConsumerRepository : IConsumerRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Consumer> dbSet;

        public ConsumerRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Consumer>();
        }


        public async Task<Consumer> GetByIdAsync(string personalAccount)
        {
            return await _context.consumers.FirstOrDefaultAsync(c => c.PersonalAccount == personalAccount);
        }

        public async Task<List<Consumer>> All()
        {
            var consumer = await dbSet.ToListAsync();
            return consumer;
        }

        public async Task<Consumer> Add(Consumer consumer)
        {
            _context.consumers.Add(consumer);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return consumer;
        }

        public async Task<Consumer> Update(Consumer consumer)
        {
            _context.Update(consumer);
            await _context.SaveChangesAsync();

            return consumer;
        }

        public async Task<Consumer> Delete(string personalAccount)
        {
            var consumer = await dbSet.FindAsync(personalAccount);
            if (consumer == null)
            {
                return null;
            }

            _context.consumers.Remove(consumer);
            await _context.SaveChangesAsync();
            return consumer;
        }

        public void GetConsumerByPersonalAccount(string v)
        {
            throw new NotImplementedException();
        }
    }
}
