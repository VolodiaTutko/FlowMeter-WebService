namespace Infrastructure.Data.Repositories
{
    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class MeterRepository : IMeterRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Meter> dbSet;

        public MeterRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Meter>();
        }

        public Task<Meter> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Meter>> All()
        {
            var consumer = await dbSet.ToListAsync();
            return consumer;
        }

        public async Task<Meter> Add(Meter meter)
        {
            _context.meters.Add(meter);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
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
