namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using System.Threading.Channels;

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

        public async Task<Service> GetServiceByServiceId(int serviceId)
        {
            return await _serviceRepository.GetByIdAsync(serviceId);
        }

        public async Task<Service> DeleteService(int id)
        {
            try
            {
                var deletedService = await _serviceRepository.Delete(id);

                _logger.LogInformation("Service with ServiceId {ServiceId} deleted successfully.", deletedService.ServiceId);
                return deletedService;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a service from the database.");
                throw; 
            }
        }
        
        public async Task<Service> UpdateService(Service service)
        {
            try
            {
                var updatedService = await _serviceRepository.Update(service);
                _logger.LogInformation("Service with ServiceId: {ServiceId} updated successfully", updatedService.ServiceId);
                return updatedService;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a consumer in the database.");
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
