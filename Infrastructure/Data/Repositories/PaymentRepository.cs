namespace Infrastructure.Data.Repositories
{
    using Application.DataAccess;
    using Application.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class PaymentRepository : IPaymentRepository
    {

        private readonly AppDbContext _context;

        protected readonly DbSet<Payment> dbSet;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
            dbSet = context.Set<Payment>();
        }

        public Task<Payment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Payment>> All()
        {
            var payment = await dbSet.ToListAsync();
            return payment;
        }
    

        public async Task<Payment> Add(Payment payment)
        {
            _context.payments.Add(payment);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return payment;
        }

        public Task<Payment> Update(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
