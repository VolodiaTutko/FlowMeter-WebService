namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IConsumerService
    {
        public Task<Consumer> AddConsumer(Consumer consumer);

        Task<Consumer> UpdateConsumer(Consumer consumer);

        Task<Consumer> DeleteConsumer(string id);

        Task<Consumer> GetConsumerByPersonalAccount(string personalAccount);

        public Task<List<Consumer>> GetList();

        Task<List<SelectConsumerDTO>> GetConsumerOptions();
    }
}
