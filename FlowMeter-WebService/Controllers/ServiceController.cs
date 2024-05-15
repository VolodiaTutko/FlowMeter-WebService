// <copyright file="ServiceController.cs" company="FlowMeter">
// Copyright (c) FlowMeter. All rights reserved.
// </copyright>

namespace FlowMeter_WebService.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Application.Models;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

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
            var serviceResult = await this.serviceService.GetList();
            if (serviceResult.IsOk)
            {
                return this.View(serviceResult.Value);
            }
            else
            {
                this.logger.LogError(serviceResult.Error.Description);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public ActionResult Details(int id)
        {
            return this.View();
        }

        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model, string houseAddress)
        {
            var houseModelResult = await this.houseService.GetHouseByAddress(houseAddress);

            this.ModelState.Remove("House");
            model.HouseId = houseModelResult.HouseId;

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index), model);
            }

            var existingDataForHouseResult = await this.serviceService.GetServiceByHouseId(houseModelResult.HouseId);
            if (existingDataForHouseResult.IsOk)
            {
                var existingService = existingDataForHouseResult.Value.FirstOrDefault(s => s.TypeOfAccount == model.TypeOfAccount && s.Price == model.Price);
                if (existingService != null)
                {
                    this.ModelState.AddModelError(string.Empty, "The service already exists for this house");
                    return this.View(model);
                }
            }
            else
            {
                this.logger.LogError(existingDataForHouseResult.Error.Description);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            var serviceResult = await this.serviceService.AddService(model);
            if (serviceResult.IsOk)
            {
                this.logger.LogInformation("Service created successfully: {ServiceId}", serviceResult.Value.ServiceId);
                this.TempData["message"] = "The service has been created successfully";
                return this.RedirectToAction(nameof(this.Index));
            }
            else
            {
                this.logger.LogError(serviceResult.Error.Description);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public ActionResult Edit(int id)
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Service service)
        {
            this.ModelState.Remove("House");
            this.ModelState.Remove("TypeOfAccount");
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var updatedServiceResult = await this.serviceService.UpdateService(service);
            if (updatedServiceResult.IsOk)
            {
                this.logger.LogInformation("Service with ServiceId: {ServiceId} updated successfully", updatedServiceResult.Value.ServiceId);
                this.TempData["message"] = "The service has been edited successfully";
                return this.RedirectToAction(nameof(this.Index));
            }
            else
            {
                this.logger.LogError(updatedServiceResult.Error.Description);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var deletedServiceResult = await this.serviceService.DeleteService(id);
            if (deletedServiceResult.IsOk)
            {
                this.logger.LogInformation("Service with ServiceId: {ServiceId} deleted successfully", deletedServiceResult.Value.ServiceId);
                this.TempData["message"] = "The service has been deleted successfully";
                return this.RedirectToAction(nameof(this.Index));
            }
            else
            {
                this.logger.LogError(deletedServiceResult.Error.Description);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
