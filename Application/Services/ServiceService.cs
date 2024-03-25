namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class ServiceService : IServiceService
    {
        private readonly ILogger<ServiceService> _logger;
        private readonly IServiceRepository _serviceRepository;
        private readonly IHouseService _houseService;

        public ServiceService(IServiceRepository serviceRepository, IHouseService houseService, ILogger<ServiceService> logger)
        {
            _serviceRepository = serviceRepository;
            _houseService = houseService;
            _logger = logger;
        }

        public async Task<Service> AddService(Service service)
        {
            try
            {
                var addedService = await _serviceRepository.Add(service);
                _logger.LogInformation("Adding a new service with ServiceId {ServiceId}.", addedService.ServiceId);
                return addedService;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a house to the database.");
                throw;
            }
        }

        public async Task<List<Service>> GetList()
        {
            try
            {
                var all = await _serviceRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                _logger.LogInformation("Retrieved {Count} services from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving services from the database.");
                throw;
            }
        }

        public async Task<List<SelectHouseDTO>> GetHouseOptions()
        {
            return await _houseService.GetHouseOptions();
        }
    }
}
