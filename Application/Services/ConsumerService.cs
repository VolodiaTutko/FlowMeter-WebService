namespace Application.Services
{
    using Application.Models;
    using Application.DataAccess;
    using Application.Services.Interfaces;

    public class ConsumerService : IConsumerService
    {
        private readonly IConsumerRepository _consumerRepository;

        public ConsumerService(IConsumerRepository consumerRepository)
        {
            _consumerRepository = consumerRepository;
        }

        public async Task<Consumer> AddConsumer(Consumer consumer)
        {
            return await _consumerRepository.Add(consumer);
        }

        public async Task<List<Consumer>> GetList()
        {
            var all = await _consumerRepository.All();
            return all.Where(item => item != null).ToList();
        }

    }
}
