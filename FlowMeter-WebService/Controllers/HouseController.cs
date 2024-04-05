namespace FlowMeter_WebService.Controllers
{
  
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class HouseController : Controller
    {
        private IHouseService _houseService;
        private readonly ILogger<HouseController> _logger;

        public HouseController(IHouseService service, ILogger<HouseController> logger)
        {
            _houseService = service;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var houses = await _houseService.GetList();
            return View(houses);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(House model)
        {
            var result = await _houseService.AddHouse(model);

            if (result.IsOk)
            {
                var addedHouse = result.Value;
                _logger.LogInformation("House created successfully with HouseAddress: {HouseAddress}", result.Value.HouseAddress);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var error = result.Error;
                _logger.LogError("Error occurred while creating house: {ErrorMessage}", error.Description);
                ModelState.AddModelError(string.Empty, $"Error: {error.Description}");
                return RedirectToAction(nameof(Index), model);
            }
        }

        public ActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(House house)
        {
            if (!ModelState.IsValid)
            {
                return View(house);
            }

            var result = await _houseService.UpdateHouse(house);

            if (result.IsOk)
            {
                _logger.LogInformation("House with {houseAddress} updated successfully", result.Value.HouseAddress);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var error = result.Error;
                _logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
                return View(house);
            }
        }


        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _houseService.DeleteHouse(id);

            if (!ModelState.IsValid)
            {
                return View(result.Value);
            }

            if (result.IsOk)
            {
                _logger.LogInformation("House with address: {HouseAddress} deleted successfully", result.Value.HouseAddress);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var error = result.Error;
                _logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
