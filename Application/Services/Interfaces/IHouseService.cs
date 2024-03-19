
using Application.Models;

namespace Application.Services.Interfaces
{
    public interface IHouseService
    {
        public Task<House> AddHouse(House house);

        public Task<List<House>> GetList();
    }
}
