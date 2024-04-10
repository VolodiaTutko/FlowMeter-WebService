namespace FlowMeter_WebService.Controllers
{
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ServiceController : Controller
    {
        private IServiceService _serviceService;
        private IHouseService _houseService;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceService service, IHouseService houseService, ILogger<ServiceController> logger)
        {
            _serviceService = service;
            _houseService = houseService;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var service = await _serviceService.GetList();
            return View(service);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create(Account a)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model, string houseAddress)
        {
            var houseModel = await _houseService.GetHouseByAddress(houseAddress);
            ModelState.Remove("House");
            model.HouseId = houseModel.HouseId;
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            try
            {
                var service = new Service
                {
                    ServiceId = model.ServiceId,
                    HouseId = model.HouseId,
                    TypeOfAccount = model.TypeOfAccount,
                    Price = model.Price
                };

                await _serviceService.AddService(service);
                _logger.LogInformation("Service created successfully: {ServiceId}", service.ServiceId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating service");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(Service service)
        {
            try
            {
                ModelState.Remove("House");
                ModelState.Remove("TypeOfAccount");
                if (ModelState.IsValid)
                {
                    var updatedService = await _serviceService.GetServiceByServiceId(service.ServiceId);

                    updatedService.Price = service.Price;

                    await _serviceService.UpdateService(updatedService);

                    if (updatedService == null)
                    {
                        return NotFound();
                    }

                    _logger.LogInformation("Service with ServiceId: {ServiceId} updated successfully", updatedService.ServiceId);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a service in the database.");
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deletedService = await _serviceService.DeleteService(id);
                if (deletedService == null)
                {
                    return NotFound(); 
                }
                _logger.LogInformation("Service with ServiceId: {ServiceId} deleted successfully", deletedService.ServiceId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting service");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return RedirectToAction(nameof(Index)); 
            }
        }
    }
}