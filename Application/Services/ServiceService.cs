// <copyright file="ServiceService.cs" company="FlowMeter">
// Copyright (c) FlowMeter. All rights reserved.
// </copyright>

namespace Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task<Result<Service, Error>> AddService(Service service)
        {
            try
            {
                var addedService = await this.serviceRepository.Add(service);
                this.logger.LogInformation("Adding a new service with ServiceId {ServiceId}.", addedService.ServiceId);
                return Result<Service, Error>.Ok(addedService);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while adding a service to the database.");
                return Result<Service, Error>.Err(new Error("DB001", "Database error occurred while adding the service."));
            }
        }

        public async Task<Result<Service, Error>> GetServiceByServiceId(int serviceId)
        {
            try
            {
                var service = await this.serviceRepository.GetByIdAsync(serviceId);
                if (service == null)
                {
                    return Result<Service, Error>.Err(new Error("NotFound", "Service not found"));
                }

                return Result<Service, Error>.Ok(service);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while retrieving a service by ID from the database.");
                return Result<Service, Error>.Err(new Error("RetrieveError", "An error occurred while retrieving a service by ID from the database."));
            }
        }

        public async Task<Result<IEnumerable<Service>, Error>> GetServiceByHouseId(int houseId)
        {
            try
            {
                var services = await this.serviceRepository.GetByHouseIdAsync(houseId);
                return Result<IEnumerable<Service>, Error>.Ok(services);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while retrieving services by house ID from the database.");
                return Result<IEnumerable<Service>, Error>.Err(new Error("RetrieveError", "An error occurred while retrieving services by house ID from the database."));
            }
        }

        public async Task<Result<Service, Error>> DeleteService(int id)
        {
            try
            {
                var deletedService = await this.serviceRepository.Delete(id);
                if (deletedService == null)
                {
                    return Result<Service, Error>.Err(new Error("NotFound", "Service not found"));
                }

                this.logger.LogInformation("Service with ServiceId {ServiceId} deleted successfully.", deletedService.ServiceId);
                return Result<Service, Error>.Ok(deletedService);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occurred while deleting service: {ex.Message}");
                return Result<Service, Error>.Err(new Error("DeleteError", $"Error occurred while deleting service: {ex.Message}"));
            }
        }

        public async Task<Result<Service, Error>> UpdateService(Service service)
        {
            try
            {
                var updatedService = await this.serviceRepository.Update(service);
                this.logger.LogInformation("Service with ServiceId: {ServiceId} updated successfully.", updatedService.ServiceId);
                return Result<Service, Error>.Ok(updatedService);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while updating a service in the database.");
                return Result<Service, Error>.Err(new Error("UpdateError", "An error occurred while updating a service in the database."));
            }
        }

        public async Task<Result<List<Service>, Error>> GetList()
        {
            try
            {
                var allServices = await this.serviceRepository.All();
                var filteredList = allServices.Where(item => item != null).ToList();
                this.logger.LogInformation("Retrieved {Count} services from the database.", filteredList.Count);
                return Result<List<Service>, Error>.Ok(filteredList);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occurred while retrieving services from the database.");
                return Result<List<Service>, Error>.Err(new Error("RetrieveError", "An error occurred while retrieving services from the database."));
            }
        }

        public async Task<Result<List<SelectHouseDTO>, Error>> GetHouseOptions()
        {
            return await this.houseService.GetHouseOptions();
        }
    }
}
