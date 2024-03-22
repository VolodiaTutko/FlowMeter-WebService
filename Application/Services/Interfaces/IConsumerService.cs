namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IConsumerService
    {
        public Task<Consumer> AddConsumer(Consumer consumer);

        public Task<List<Consumer>> GetList();
        Task<List<SelectConsumerDTO>> GetConsumerOptions();
    }
}
