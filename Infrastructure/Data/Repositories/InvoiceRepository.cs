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
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<Receipt> dbSet;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Receipt>();
        }


        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await _context.receipts.FirstOrDefaultAsync(c => c.ReceiptId == id);
        }

        public async Task<List<Receipt>> All()
        {
            var invoice = await dbSet.Include(r => r.Consumer).ThenInclude(c => c.House).OrderBy(x => x.ReceiptId).ToListAsync(); ;
            return invoice;
        }
    }
}
