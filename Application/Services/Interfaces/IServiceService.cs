namespace Application.Services.Interfaces
{
    using Application.Models;

    public interface IServiceService
    {
        public Task<Service> AddService(Service service);

        Task<Service> DeleteService(int id);

        Task<Service> UpdateService(Service service);

        Task<Service> GetServiceByServiceId(int serviceId);

        Task<IEnumerable<Service>> GetServiceByHouseId(int houseId);

        public Task<List<Service>> GetList();
    }
}
