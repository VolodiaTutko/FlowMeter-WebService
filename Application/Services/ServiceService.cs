namespace Application.Services
{
    using Application.DataAccess;
    using Application.DTOS;
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class ServiceService : IServiceService
    {
        private readonly ILogger<ServiceService> logger;
        private readonly IServiceRepository serviceRepository;
        private readonly IHouseService houseService;

        public ServiceService(IServiceRepository serviceRepository, IHouseService houseService, ILogger<ServiceService> logger)
        {
            this.serviceRepository = serviceRepository;
            this.houseService = houseService;
            this.logger = logger;
        }

        public async Task<Service> AddService(Service service)
        {
            try
            {
                var addedService = await this.serviceRepository.Add(service);
                this.logger.LogInformation("Adding a new service with ServiceId {ServiceId}.", addedService.ServiceId);
                return addedService;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while adding a house to the database.");
                throw;
            }
        }

        public async Task<Service> GetServiceByServiceId(int serviceId)
        {
            return await this.serviceRepository.GetByIdAsync(serviceId);
        }

        public async Task<IEnumerable<Service>> GetServiceByHouseId(int houseId)
        {
            return await this.serviceRepository.GetByHouseIdAsync(houseId);
        }

        public async Task<Service> DeleteService(int id)
        {
            try
            {
                var deletedService = await this.serviceRepository.Delete(id);

                this.logger.LogInformation("Service with ServiceId {ServiceId} deleted successfully.", deletedService.ServiceId);
                return deletedService;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while deleting a service from the database.");
                throw;
            }
        }

        public async Task<Service> UpdateService(Service service)
        {
            try
            {
                var updatedService = await this.serviceRepository.Update(service);
                this.logger.LogInformation("Service with ServiceId: {ServiceId} updated successfully", updatedService.ServiceId);
                return updatedService;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while updating a consumer in the database.");
                throw;
            }
        }

        public async Task<List<Service>> GetList()
        {
            try
            {
                var all = await this.serviceRepository.All();
                var filteredList = all.Where(item => item != null).ToList();
                this.logger.LogInformation("Retrieved {Count} services from the database.", filteredList.Count);
                return filteredList;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while retrieving services from the database.");
                throw;
            }
        }

        public async Task<List<SelectHouseDTO>> GetHouseOptions()
        {
            return await this.houseService.GetHouseOptions();
        }
    }
}
