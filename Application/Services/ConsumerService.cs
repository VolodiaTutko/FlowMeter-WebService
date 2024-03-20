namespace Application.Services
{
    using Application.Models;
    using Application.DataAccess;
    using Application.Services.Interfaces;
    using Application.DTOS;

    public class ConsumerService : IConsumerService
    {
        private readonly IConsumerRepository _consumerRepository;
        private readonly IHouseService _houseService;

        public ConsumerService(IConsumerRepository consumerRepository, IHouseService houseService)
        {
            _consumerRepository = consumerRepository;
            _houseService = houseService;
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

        public async Task<List<SelectHouseDTO>> GetHouseOptions() 
        {
            return await _houseService.GetHouseOptions();
        }
    }
}
