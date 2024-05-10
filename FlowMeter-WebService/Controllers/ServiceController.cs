namespace FlowMeter_WebService.Controllers
{
    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IServiceService serviceService;
        private readonly IHouseService houseService;
        private readonly ILogger<ServiceController> logger;

        public ServiceController(IServiceService service, IHouseService houseService, ILogger<ServiceController> logger)
        {
            this.serviceService = service;
            this.houseService = houseService;
            this.logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var service = await this.serviceService.GetList();
            return this.View(service);
        }

        public ActionResult Details(int id)
        {
            return this.View();
        }

        public ActionResult Create(Account a)
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model, string houseAddress)
        {
            var houseModel = await this.houseService.GetHouseByAddress(houseAddress);
            this.ModelState.Remove("House");
            model.HouseId = houseModel.HouseId;
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index), model);
            }

            var service = new Service
            {
                ServiceId = model.ServiceId,
                HouseId = model.HouseId,
                TypeOfAccount = model.TypeOfAccount,
                Price = model.Price,
            };

            await this.serviceService.AddService(service);
            this.logger.LogInformation("Service created successfully: {ServiceId}", service.ServiceId);
            this.TempData["message"] = "The service has been created successfully";

            return this.RedirectToAction(nameof(this.Index));
        }

        public ActionResult Edit(int id)
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
             return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(Service service)
        {
            this.ModelState.Remove("House");
            this.ModelState.Remove("TypeOfAccount");
            if (this.ModelState.IsValid)
            {
                var updatedService = await this.serviceService.GetServiceByServiceId(service.ServiceId);

                updatedService.Price = service.Price;

                await this.serviceService.UpdateService(updatedService);

                if (updatedService == null)
                {
                    return this.NotFound();
                }

                this.logger.LogInformation("Service with ServiceId: {ServiceId} updated successfully", updatedService.ServiceId);
                this.TempData["message"] = "The service has been edited successfuly";

                return this.RedirectToAction(nameof(this.Index));
            }
            else
            {
                return this.RedirectToAction(nameof(this.Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var deletedService = await this.serviceService.DeleteService(id);
            if (deletedService == null)
            {
                return this.NotFound();
            }

            this.logger.LogInformation("Service with ServiceId: {ServiceId} deleted successfully", deletedService.ServiceId);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
