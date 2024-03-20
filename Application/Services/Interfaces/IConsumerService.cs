using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IConsumerService
    {
        public Task<Consumer> AddConsumer(Consumer consumer);

        public Task<List<Consumer>> GetList();
    }
}
