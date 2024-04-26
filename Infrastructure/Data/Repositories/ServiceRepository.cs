// <copyright file="ServiceRepository.cs" company="FlowMeter">
// Copyright (c) FlowMeter. All rights reserved.
// </copyright>

namespace Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext context;
        private readonly DbSet<Service> dbSet;

        public ServiceRepository(AppDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<Service>();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            return await this.context.services
                .FirstOrDefaultAsync(c => c.ServiceId == id);
        }

        public async Task<IEnumerable<Service>> GetByHouseIdAsync(int id)
        {
            return await this.context.services
                .Where(c => c.HouseId == id)
                .ToListAsync();
        }

        public async Task<List<Service>> All()
        {
            var service = await this.dbSet.ToListAsync();
            return service;
        }

        public async Task<Service> Add(Service service)
        {
            try
            {
                this.context.services.Add(service);
                await this.context.SaveChangesAsync();
                return service;
            }
            catch (Exception ex)
            {
                throw new Exception("Помилка при додаванні сервісу до бази даних.", ex);
            }
        }

        public async Task<Service> Update(Service service)
        {
            this.context.Update(service);
            await this.context.SaveChangesAsync();

            return service;
        }

        public async Task<Service> Delete(int serviceId)
        {
            var service = await this.dbSet.FindAsync(serviceId);
            if (service == null)
            {
                return null;
            }

            this.context.services.Remove(service);
            await this.context.SaveChangesAsync();
            return service;
        }
    }
}
