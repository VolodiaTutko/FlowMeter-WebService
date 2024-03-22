namespace Application.DataAccess
{
    using Application.Models;

    public interface IMeterRepository
    {
        Task<Meter> GetByIdAsync(int id);

        Task<List<Meter>> All();

        Task<Meter> Add(Meter meter);

        Task<Meter> Update(Meter meter);

        Task<Meter> Delete(int id);
    }
}
