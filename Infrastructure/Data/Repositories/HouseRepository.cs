namespace Infrastructure.Data.Repositories
{
    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class HouseRepository : IHouseRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<House> dbSet;

        public HouseRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<House>();
        }

        public Task<House> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<House>> All()
        {
            var houses = await dbSet.ToListAsync();
            return houses;
        }

        public async Task<House> Add(House house)
        {
            _context.houses.Add(house);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return house;
        }

        public Task<House> Update(House house)
        {
            throw new NotImplementedException();
        }

        public Task<House> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
