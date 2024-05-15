// <copyright file="IServiceService.cs" company="FlowMeter">
// Copyright (c) FlowMeter. All rights reserved.
// </copyright>

namespace Application.Services.Interfaces
{
    using Application.Models;

    public interface IServiceService
    {
        public Task<Result<Service, Error>> AddService(Service service);

        Task<Result<Service, Error>> DeleteService(int id);

        Task<Result<Service, Error>> UpdateService(Service service);

        Task<Result<Service, Error>> GetServiceByServiceId(int serviceId);

        Task<Result<IEnumerable<Service>, Error>> GetServiceByHouseId(int houseId);

        public Task<Result<List<Service>, Error>> GetList();
    }
}
