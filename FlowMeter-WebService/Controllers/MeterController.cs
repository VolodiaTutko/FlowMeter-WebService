﻿namespace FlowMeter_WebService.Controllers
{
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class MeterController : Controller
    {
        private IMeterService meterService;
        private readonly ILogger<MeterController> logger;

        public MeterController(IMeterService meterService, ILogger<MeterController> logger)
        {
            this.meterService = meterService;
            this.logger = logger;
        }

        // GET: MeterController
        public async Task<ActionResult> Index()
        {
            var meters = await this.meterService.GetList();
            return View(meters);
        }

        

        // POST: MeterController/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deletedMeter = await this.meterService.DeleteMeter(id);
                if (deletedMeter == null)
                {
                    return this.NotFound();
                }

                this.logger.LogInformation("Meter with id: {MeterId} deleted successfully", deletedMeter.MeterId);
                return this.RedirectToAction(nameof(this.Index));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while deleting meter with id: {id}", id);
                this.ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return this.RedirectToAction(nameof(this.Index));
            }
        }

        // GET: MeterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MeterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: MeterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MeterController/Edit/5
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
    }
}