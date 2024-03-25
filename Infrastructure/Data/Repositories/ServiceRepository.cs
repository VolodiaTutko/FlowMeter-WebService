namespace Infrastructure.Data.Repositories
{
    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Service> dbSet;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Service>();
        }

        public Task<Service> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Service>> All()
        {
            var service = await dbSet.ToListAsync();
            return service;
        }

        public async Task<Service> Add(Service service)
        {
            try
            {
                _context.services.Add(service);
                await _context.SaveChangesAsync();
                return service;
            }
            catch (Exception ex)
            {
                // Обробка помилки
                throw new Exception("Помилка при додаванні сервісу до бази даних.", ex);
            }
        }

        public Task<Service> Update(Service service)
        {
            throw new NotImplementedException();
        }

        public Task<Service> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
