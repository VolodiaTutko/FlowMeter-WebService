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
    public class MeterRepository : IMeterRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Meter> dbSet;

        public MeterRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Meter>();
        }

        public async Task<Meter> GetByCounterAccountAsync(string id)
        {
            return await _context.meters.FirstOrDefaultAsync(c => c.CounterAccount == id);
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

        public async Task<Meter> Update(Meter meter)
        {
            var existingMeter = await _context.meters.FindAsync(meter.CountersId);
            if (existingMeter == null)
            {
                throw new InvalidOperationException("Meter not found");
            }

            existingMeter.CurrentIndicator = meter.CurrentIndicator;
            existingMeter.TypeOfAccount = meter.TypeOfAccount;
            existingMeter.Role = meter.Role;
            existingMeter.Date = meter.Date;

            await _context.SaveChangesAsync();
            return existingMeter;
        }

        public async Task<Meter> Delete(int id)
        {
            var meter = await _context.meters.FindAsync(id);
            if (meter == null)
            {
                throw new InvalidOperationException("Meter not found");
            }

            _context.meters.Remove(meter);
            await _context.SaveChangesAsync();
            return meter;
        }

    }
}
