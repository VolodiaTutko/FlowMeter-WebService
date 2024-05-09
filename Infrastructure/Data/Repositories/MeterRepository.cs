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
    using System.Collections.Generic;

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
            var meters = await _dbSet.ToListAsync();
            return meters;
        }

        public async Task<Meter> Add(Meter meter)
        {
            this._dbSet.Add(meter);
            this._context.SaveChanges();
            return meter;
        }
        public async Task<Meter> Update(Meter meter)
        {
            this._context.Update(meter);
            await this._context.SaveChangesAsync();

            return meter;
        }
        public async Task<Meter> Delete(int id)
        {
            var meter = await this._dbSet.FindAsync(id);
            if (meter == null)
            {
                throw new KeyNotFoundException();
            }

            var typeOfAccount = meter.TypeOfAccount;

            Account account = null;
            switch (typeOfAccount)
            {
                case "HotWater":
                    account = await _context.accounts.FirstOrDefaultAsync(a => a.HotWater == meter.CounterAccount);
                    break;
                case "ColdWater":
                    account = await _context.accounts.FirstOrDefaultAsync(a => a.ColdWater == meter.CounterAccount);
                    break;
                case "Heating":
                    account = await _context.accounts.FirstOrDefaultAsync(a => a.Heating == meter.CounterAccount);
                    break;
                case "Electricity":
                    account = await _context.accounts.FirstOrDefaultAsync(a => a.Electricity == meter.CounterAccount);
                    break;
                case "Gas":
                    account = await _context.accounts.FirstOrDefaultAsync(a => a.Gas == meter.CounterAccount);
                    break;
                case "PublicService":
                    account = await _context.accounts.FirstOrDefaultAsync(a => a.PublicService == meter.CounterAccount);
                    break;
                default:
                    break;
            }

            if (account != null)
            {
                switch (typeOfAccount)
                {
                    case "HotWater":
                        account.HotWater = null;
                        break;
                    case "ColdWater":
                        account.ColdWater = null;
                        break;
                    case "Heating":
                        account.Heating = null;
                        break;
                    case "Electricity":
                        account.Electricity = null;
                        break;
                    case "Gas":
                        account.Gas = null;
                        break;
                    case "PublicService":
                        account.PublicService = null;
                        break;
                    default:
                        break;
                }
            }

            _context.meters.Remove(meter);
            await this._context.SaveChangesAsync();
            return meter;
        }

        public async Task<Meter> GetByCounterAccountAsync(string id)
        {
            return await _context.meters
                        .FirstOrDefaultAsync(r => r.CounterAccount == id);
        }

		public async Task<Meter> GetByTypeOfAccountAsync(string id)
		{
			return await _context.meters
						.FirstOrDefaultAsync(r => r.TypeOfAccount == id);
		}
	}
}
