using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowMeter_WebService.Controllers
{
    using Application.Services;
    using Application.Models;
    using Application.Services.Interfaces;

    public class ConsumerController : Controller
    {
        private IConsumerService _consumerService;

        public ConsumerController(IConsumerService service)
        {
            _consumerService = service;
        }

        public async Task<ActionResult> Index()
        {
            var consumer = await _consumerService.GetList();
            return View(consumer);
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
        public async Task<IActionResult> Create(Consumer model)
        {
            ModelState.Remove("House");
            ModelState.Remove("User");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            try
            {
                var consumer = new Consumer
                {
                    PersonalAccount = model.PersonalAccount,
                    Flat = model.Flat,
                    ConsumerOwner = model.ConsumerOwner,
                    HeatingArea = model.HeatingArea,
                    HouseId = model.HouseId,
                    NumberOfPersons = model.NumberOfPersons,
                    ConsumerEmail = model.ConsumerEmail
                };

                await _consumerService.AddConsumer(consumer);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
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
