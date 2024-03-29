namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IServiceService
    {
        public Task<Service> AddService(Service service);

        Task<Service> DeleteService(int id);

        Task<Service> UpdateService(Service service);

        Task<Service> GetServiceByServiceId(int serviceId);

        public Task<List<Service>> GetList();
    }
}
