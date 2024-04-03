
namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;
    using Microsoft.AspNetCore.Http.HttpResults;

    public interface IHouseService
    {
        Task<Result<House, Error>> AddHouse(House house);

        Task<Result<House, Error>> UpdateHouse(House house);

        Task<Result<House, Error>> DeleteHouse(int id);

        Task<House> GetHouseById(int id);

        Task<House> GetHouseByAddress(string houseAddress);

        Task<List<House>> GetList();

        Task<List<SelectHouseDTO>> GetHouseOptions();
    }
}
