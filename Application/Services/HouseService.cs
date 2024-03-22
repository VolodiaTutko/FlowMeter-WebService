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
            return await _houseRepository.Add(house);
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
