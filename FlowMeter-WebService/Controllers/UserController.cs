using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlowMeter_WebService.Controllers
{
    public class UserController : Controller
    {
        private IConsumerService _consumerService;
        private IHouseService _houseService;

        public UserController(IConsumerService consumerService, IHouseService houseService)
        {
            _consumerService = consumerService;
            _houseService = houseService;
        }

        public async Task<IActionResult> Index()
        {
            string personalAccountToFind = "2343252333";

            var consumer = await _consumerService.GetConsumerByPersonalAccount(personalAccountToFind);

            if (consumer != null)
            {
                var house = await _houseService.GetHouseById(consumer.HouseId);

                ViewBag.HouseAddress = house?.HouseAddress;

                return View(consumer);
            }
            else
            {
                return NotFound();
            }

        }
    }
}