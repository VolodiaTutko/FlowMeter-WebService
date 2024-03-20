namespace Application.DataAccess
{
    using Application.Models;

    public interface IHouseRepository
    {
        Task<House> GetByIdAsync(int id);

        Task<List<House>> All();

        Task<House> Add(House house);

        Task<House> Update(House house);

        Task<House> Delete(int id);
    }
}
