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
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceService service, ILogger<ServiceController> logger)
        {
            _serviceService = service;
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
        public async Task<IActionResult> Create(Service model)
        {
            ModelState.Remove("House");
            //ModelState.Remove("User");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            try
            {
                var service = new Service
                {
                    HouseId = model.HouseId,
                    TypeOfAccount = GetUkrainianTypeOfAccount(model.TypeOfAccount), // Отримати українське значення
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

        private string GetUkrainianTypeOfAccount(string typeOfAccount)
        {
            switch (typeOfAccount)
            {
                case "ColdWater":
                    return "Холодна вода";
                case "HotWater":
                    return "Гаряча вода";
                case "Gas":
                    return "Газ";
                case "Electricity":
                    return "Світло";
                default:
                    return string.Empty;
            }
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