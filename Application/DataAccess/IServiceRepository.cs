namespace Application.DataAccess
{
    using Application.Models;

    public interface IServiceRepository
    {
        Task<Service> GetByIdAsync(int id);

        Task<List<Service>> All();

        Task<Service> Add(Service service);

        Task<Service> Update(Service service);

        Task<Service> Delete(int id);
    }
}
