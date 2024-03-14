
namespace Application.UseCases.House.Add
{
    using Application.DataAccess;
    using Core.Models;
    public class AddHouse
    {
        private readonly IHouseRepository _houseRepository;

        public AddHouse(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public async Task<House> Execute(House house)
        {
            return await _houseRepository.Add(house);
        }
    }
}
