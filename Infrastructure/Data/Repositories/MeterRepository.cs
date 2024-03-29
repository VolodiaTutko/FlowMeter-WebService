namespace Infrastructure.Data.Repositories
{
    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class MeterRepository : IMeterRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Meter> _dbSet;

        public MeterRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Meter>();
        }

        public async Task<Meter> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.MeterId == id);
        }

        public async Task<List<Meter>> All()
        {
            var consumer = await _dbSet.ToListAsync();
            return consumer;
        }

        public async Task<Meter> Add(Meter meter)
        {
            _context.meters.Add(meter);
            _context.SaveChanges();
            return meter;
        }

        public Task<Meter> Update(Meter meter)
        {
            throw new NotImplementedException();
        }

        public Task<Meter> Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
