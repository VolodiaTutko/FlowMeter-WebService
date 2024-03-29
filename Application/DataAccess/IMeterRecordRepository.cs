namespace Application.DataAccess
{
    using Application.Models;

    public interface IMeterRecordRepository
    {
        Task<MeterRecord> GetByIdAsync(int id);

        Task<List<MeterRecord>> All();

        Task<List<MeterRecord>> GetListByMeterId(int meterId);

        Task<MeterRecord> Add(MeterRecord record);
    }
}
