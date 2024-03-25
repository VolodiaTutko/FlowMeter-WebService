namespace Application.DataAccess
{
    using Application.Models;

    public interface IConsumerRepository
    {
        Task<Consumer> GetByIdAsync(string id);

        Task<List<Consumer>> All();

        Task<Consumer> Add(Consumer consumer);

        Task<Consumer> Update(Consumer consumer);

        Task<Consumer> Delete(string id);
        void GetConsumerByPersonalAccount(string v);
    }
}
