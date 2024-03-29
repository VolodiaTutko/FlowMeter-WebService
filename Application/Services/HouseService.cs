namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
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

        public async Task<House> AddHouse(House house)
        {
            try
            {
                var addedHouse = await _houseRepository.Add(house);
                _logger.LogInformation("Added a new house to the database with ID: {HouseId}", addedHouse.HouseId);
                return addedHouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a house to the database.");
                throw;
            }
        }

        public async Task<House> UpdateHouse(House house)
        {
            try
            {
                var updatedHouse = await _houseRepository.Update(house);
                _logger.LogInformation("House with Address: {HouseAddress} updated successfully", updatedHouse.HouseAddress);
                return updatedHouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a house in the database.");
                throw;
            }
        }

        public async Task<House> DeleteHouse(int id)
        {
            try
            {
                var deletedHouse = await _houseRepository.Delete(id);
                _logger.LogInformation("House with Address: {HouseAddress} deleted successfully", deletedHouse.HouseAddress);
                return deletedHouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a house from the database.");
                throw;
            }
        }

        public async Task<House> GetHouseById(int id)
        {
            return await _houseRepository.GetByIdAsync(id);
        }

        public async Task<House> GetHouseByAddress(string houseAddress)
        {
            return await _houseRepository.GetByAddress(houseAddress);   
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
