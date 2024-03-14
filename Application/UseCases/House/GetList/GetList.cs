namespace Application.UseCases.House.GetList
{
    using Application.DataAccess;
    using Core.Models;
    public class GetList
    {
        private readonly IHouseRepository _houseRepository;

        public GetList(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public async Task<List<House>> Execute()
        {
            return await _houseRepository.All();
        }
    }
}
