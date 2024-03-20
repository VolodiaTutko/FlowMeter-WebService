namespace Application.DataAccess
{
    using Application.Models;

    public interface IConsumerRepository
    {
        Task<Consumer> GetByIdAsync(int id);

        Task<List<Consumer>> All();

        Task<Consumer> Add(Consumer consumer);

        Task<Consumer> Update(Consumer consumer);

        Task<Consumer> Delete(int id);
    }
}
