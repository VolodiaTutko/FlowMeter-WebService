namespace FlowMeter_WebService.Controllers
{
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

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
            //ModelState.Remove("House");
            //ModelState.Remove("User");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            try
            {
                var house = new House
                {
                    HouseAddress = model.HouseAddress,
                    HeatingAreaOfHouse = model.HeatingAreaOfHouse,
                    NumberOfFlat = model.NumberOfFlat,
                    NumberOfResidents = model.NumberOfResidents,
                };

                await _houseService.AddHouse(house);
                _logger.LogInformation("House created successfully: {HouseAddress}", house.HouseAddress);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating house");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            return RedirectToAction(nameof(Index), model);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
