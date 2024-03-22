namespace Application.DataAccess
{
    using Application.Models;
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);

        Task<List<Payment>> All();

        Task<Payment> Add(Payment house);

        Task<Payment> Update(Payment house);

        Task<Payment> Delete(int id);
    }
}
