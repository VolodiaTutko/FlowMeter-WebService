namespace FlowMeter_WebService.Controllers
{
    using Application.DTOS;
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    [Authorize]
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
        [Authorize(Roles = "Admin")]
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
            var deletedMeter = await this.meterService.DeleteMeter(id);
            if (deletedMeter == null)
            {
                return this.NotFound();
            }

            this.logger.LogInformation("Meter with id: {MeterId} deleted successfully", deletedMeter.MeterId);
            return this.RedirectToAction(nameof(this.Index));
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
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            var consumerValue = collection["Consumer"];
            var serviceTypeValue = collection["ServiceType"];
            var dateValue = DateTime.Parse(collection["date"]);
            var indicatorValue = int.Parse(collection["indicator"]);

            var createvm = new CreateMeterVm(serviceTypeValue, indicatorValue, dateValue, consumerValue);

            var meter = await meterService.RegisterMeter(createvm);

            return RedirectToAction(nameof(Index));
        }

        // POST: MeterController/RegisterRecordByAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterRecordByAdmin(int id, IFormCollection collection)
        {
            int MeterId = id;
            var dateValue = DateTime.Parse(collection["date"]);
            var indicatorValue = int.Parse(collection["indicator"]);

            var record = new NewMeterRecordVm(indicatorValue, dateValue, MeterId);

            await meterService.RegisterRecordAdmin(record);

            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }
    }
}
