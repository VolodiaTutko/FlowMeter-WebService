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
            var result = await _houseService.AddHouse(model);

            if (result.IsOk)
            {
                var addedHouse = result.Value;
                _logger.LogInformation("House created successfully with HouseAddress: {HouseAddress}", addedHouse.HouseAddress);
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
        public async Task<ActionResult> Update(House house)
        {
            try
            {
                ModelState.Remove("Flat");
                ModelState.Remove("HeatingArea");
                ModelState.Remove("HouseId");
                ModelState.Remove("ConsumerEmail");
                ModelState.Remove("House");
                if (ModelState.IsValid)
                {
                    var updatedHouse = await _houseService.GetHouseById(house.HouseId);

                    updatedHouse.HouseAddress = house.HouseAddress != null ? house.HouseAddress : updatedHouse.HouseAddress;
                    updatedHouse.HeatingAreaOfHouse = house.HeatingAreaOfHouse != null ? house.HeatingAreaOfHouse : updatedHouse.HeatingAreaOfHouse;
                    updatedHouse.NumberOfFlat = house.NumberOfFlat != null ? house.NumberOfFlat : updatedHouse.NumberOfFlat;
                    updatedHouse.NumberOfResidents = house.NumberOfResidents != null ? house.NumberOfResidents : updatedHouse.NumberOfResidents;

                    await _houseService.UpdateHouse(updatedHouse);

                    if (updatedHouse == null)
                    {
                        return NotFound();
                    }

                    _logger.LogInformation("House with Address: {HouseAddress} updated successfully", updatedHouse.HouseAddress);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a house in the database.");
                throw;
            }
        }

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deletedHouse = await _houseService.DeleteHouse(id);
                if (deletedHouse == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("House with address: {HouseAddress} deleted successfully", deletedHouse.HouseAddress);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting house");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
