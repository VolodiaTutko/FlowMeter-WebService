namespace Application.Services.Interfaces
{
    using Application.DTOS;
    using Application.Models;

    public interface IServiceService
    {
        public Task<Service> AddService(Service service);

        public Task<List<Service>> GetList();
        //Task<List<SelectServiceDTO>> GetConsumerOptions();
    }
}
