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
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Service> dbSet;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Service>();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            return await _context.services.FirstOrDefaultAsync(c => c.ServiceId == id);
        }

        public async Task<IEnumerable<Service>> GetByHouseIdAsync(int id)
        {
            return await _context.services
                .Where(c => c.HouseId == id)
                .ToListAsync();
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
                throw new Exception("Помилка при додаванні сервісу до бази даних.", ex);
            }
        }

        public async Task<Service> Update(Service service)
        {
            _context.Update(service);
            await _context.SaveChangesAsync();

            return service;
        }


        public async Task<Service> Delete(int serviceId)
        {
            var service = await dbSet.FindAsync(serviceId);
            if (service == null)
            {
                return null;
            }

            _context.services.Remove(service);
            await _context.SaveChangesAsync();
            return service;
        }
    }
}
