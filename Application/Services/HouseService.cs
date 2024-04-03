namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.Extensions.Logging;

    public class HouseService: IHouseService
    {
        private readonly ILogger<HouseService> _logger;
        private readonly IHouseRepository _houseRepository;

        public HouseService(IHouseRepository houseRepository, ILogger<HouseService> logger)
        {
            _houseRepository = houseRepository;
            _logger = logger;
        }

        public async Task<Result<House, Error>> AddHouse(House house)
        {
            try
            {
                var addedHouse = await _houseRepository.Add(house);
                return Result<House, Error>.Ok(addedHouse);
            }
            catch (Exception ex)
            {
                return Result<House, Error>.Err(new Error("DB001", "Database error occurred while adding the house."));
            }
        }


        public async Task<Result<House, Error>> UpdateHouse(House house)
        {
            try
            {
                var updatedHouse = await _houseRepository.Update(house);

                
                return Result<House, Error>.Ok(updatedHouse);
            }
            catch (Exception ex)
            {
                return Result<House, Error>.Err(new Error("DB002", "Database error occurred while updating the house."));
            }
        }

        public async Task<Result<House, Error>> DeleteHouse(int id)
        {
            try
            {
                var deletedHouse = await _houseRepository.Delete(id);
                if (deletedHouse == null)
                {
                    return Result<House, Error>.Err(new Error("NotFound", "House not found"));
                }

                return Result<House, Error>.Ok(deletedHouse);
            }
            catch (Exception ex)
            {
                return Result<House, Error>.Err(new Error("DeleteError", $"Error occurred while deleting house: {ex.Message}"));
            }
        }

        public async Task<House> GetHouseById(int id)
        {
            return await _houseRepository.GetByIdAsync(id);
        }

        public async Task<List<House>> GetList()
        {
            try
            {
                var all = await _houseRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} houses from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving houses from the database.");
                throw;
            }
        }

        public async Task<List<SelectHouseDTO>> GetHouseOptions()
        {
            var allHouses = await _houseRepository.All();
            List<SelectHouseDTO> options = new List<SelectHouseDTO>();
            allHouses.ForEach(item => options.Add(new SelectHouseDTO(item)));
            return options;
        }

        
    }
}
