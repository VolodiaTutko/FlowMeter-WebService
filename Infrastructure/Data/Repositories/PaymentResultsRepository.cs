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
    public class PaymentResultsRepository : IPaymentResultsRepository
    {

        private readonly AppDbContext _context;
        protected readonly DbSet<Accrual> dbSet;

        public PaymentResultsRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Accrual>();
        }
        public async Task<List<Accrual>> All()
        {
            var accrual = await dbSet.ToListAsync();
            return accrual;
        }

        public async Task<Accrual> GetByIdAsync(int id)
        {
            return await _context.accruals.FirstOrDefaultAsync(c => c.AccrualID == id);
        }

        public async Task<IEnumerable<Accrual>> GetByPersonalAccountAsync(string personalAccount)
        {
            return await _context.accruals
                        .Where(r => r.Consumer.PersonalAccount == personalAccount)
                        .ToListAsync();
        }
    }
}
