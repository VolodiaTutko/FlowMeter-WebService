using Moq;
using Application.DataAccess;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.Logging;

namespace TestProject
{
    

    public class HouseServiceTests
    {
        private readonly HouseService _houseService;
        private readonly Mock<IHouseRepository> _mockHouseRepository;
        private readonly Mock<ILogger<HouseService>> _mockLogger;

        public HouseServiceTests()
        {
            _mockHouseRepository = new Mock<IHouseRepository>();
            _mockLogger = new Mock<ILogger<HouseService>>();

            _houseService = new HouseService(_mockHouseRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddHouse_ReturnsSuccessResult()
        {
            
            var house = new House();
            var addedHouse = new House();
            _mockHouseRepository.Setup(repo => repo.Add(It.IsAny<House>())).ReturnsAsync(addedHouse);            
            var result = await _houseService.AddHouse(house);

            
            Assert.True(result.IsOk);
            Assert.Equal(addedHouse, result.Value);
        }

        [Fact]
        public async Task UpdateHouse_ReturnsSuccessResult()
        {
            var house = new House();
            var updatedHouse = new House();
            _mockHouseRepository.Setup(repo => repo.Update(It.IsAny<House>())).ReturnsAsync(updatedHouse);
            var result = await _houseService.UpdateHouse(house);


            Assert.True(result.IsOk);
            Assert.Equal(updatedHouse, result.Value);
        }

        [Fact]
        public async Task DeleteHouse_ReturnsSuccessResult()
        {
            var id = 1;
            var deletedHouse = new House();
            _mockHouseRepository.Setup(repo => repo.Delete(id)).ReturnsAsync(deletedHouse);
            var result = await _houseService.DeleteHouse(id);

            Assert.True(result.IsOk);
            Assert.Equal(deletedHouse, result.Value);
        }

        [Fact]
        public async Task GetHouseById_ReturnsHouse()
        {
            var id = 1;
            var house = new House();
            _mockHouseRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(house);
            var result = await _houseService.GetHouseById(id);

            Assert.Equal(house, result);
        }

        [Fact]
        public async Task GetHouseByAddress_ReturnsHouse()
        {
            
            var address = "123 Main St";
            var house = new House();
            _mockHouseRepository.Setup(repo => repo.GetByAddress(address)).ReturnsAsync(house);           
            var result = await _houseService.GetHouseByAddress(address);            

            Assert.Equal(house, result);
        }


    }

}
