namespace Infrastructure.Data.Repositories
{
    using System.Collections.Generic;

    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;

    public class MeterRecordRepository : IMeterRecordRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<MeterRecord> _dbSet;

        public MeterRecordRepository(AppDbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<MeterRecord>();
        }

        public async Task<MeterRecord> GetByIdAsync(int id)
        {
            var meterRecord = await this._dbSet.FirstOrDefaultAsync(c => c.MeterRecordId == id);
            if (meterRecord == null)
            {
                throw new KeyNotFoundException();
            }
            return meterRecord;
        }

        public async Task<List<MeterRecord>> All()
        {
            var meters = await this._dbSet.ToListAsync();
            return meters;
        }

        public async Task<List<MeterRecord>> GetListByMeterId(int meterId)
        {
            var meters = await this._dbSet.Where(cr => cr.Meter.MeterId == meterId).ToListAsync();
            return meters;
        }

        public async Task<MeterRecord> Add(MeterRecord record)
        {
            await this._dbSet.AddAsync(record);
            await this._context.SaveChangesAsync();
            return record;
        }

    }
}
