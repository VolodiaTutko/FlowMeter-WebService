namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;
    using Application.ViewModels;

    public interface IConsumerService
    {
        Task<Consumer> AddConsumer(Consumer consumer);

        Task<Consumer> CreateConsumer(Consumer model, string houseAddress);

        Task<Consumer> UpdateConsumer(ConsumerUpdateViewModel consumer);

        Task<Consumer> DeleteConsumer(string id);

        Task<Consumer> GetConsumerByPersonalAccount(string personalAccount);

        Task<Consumer> GetConsumerByEmail(string consumerEmail);

        public Task<List<Consumer>> GetList();

        Task<List<SelectConsumerDTO>> GetConsumerOptions();
    }
}
