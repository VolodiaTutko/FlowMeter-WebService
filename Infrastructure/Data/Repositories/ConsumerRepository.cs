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

        public Task<Consumer> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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
            await dbSet.AddAsync(consumer);
            await _context.SaveChangesAsync();
            return await Task.FromResult(consumer);
        }

        public Task<Consumer> Update(Consumer consumer)
        {
            throw new NotImplementedException();
        }

        public Task<Consumer> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}