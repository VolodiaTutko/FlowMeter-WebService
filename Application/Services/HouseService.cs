
namespace Application.Services
{
    using Application.Models;
    using Application.DataAccess;
    using Application.Services.Interfaces;
    public class HouseService: IHouseService
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
            var all = await _houseRepository.All();
            return all.Where(item => item != null).ToList();
        }
    }
}
