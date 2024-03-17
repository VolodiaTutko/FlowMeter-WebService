
namespace Application.Services
{
    using Application.Models;
    using Application.DataAccess;
    public class HouseService
    {
        private readonly IHouseRepository _houseRepository;

        public HouseService(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }   

        public async Task<House> AddHouse(House house)
        {
            return await _houseRepository.Add(house);
        }

        public async Task<List<House>> GetList()
        {
            return await _houseRepository.All();
        }
    }
}
