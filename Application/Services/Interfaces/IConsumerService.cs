namespace Application.Services.Interfaces
{
    using Application.Models;

    public interface IConsumerService
    {
        public Task<Consumer> AddConsumer(Consumer consumer);

        public Task<List<Consumer>> GetList();
    }
}
