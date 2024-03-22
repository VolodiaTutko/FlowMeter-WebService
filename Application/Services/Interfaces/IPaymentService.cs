namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;
    public interface IPaymentService
    {
        public Task<Payment> AddPayment(Payment payment);
        public Task<List<Payment>> GetList();

    }
}
