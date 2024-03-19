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
            var houses = await _consumerService.GetList();
            return View(houses);
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
            try
            {
                // Використовуйте отриману модель, щоб створити новий об'єкт Consumer
                var consumer = new Consumer
                {
                    PersonalAccount = model.PersonalAccount,
                    Flat = model.Flat,
                    ConsumerOwner = model.ConsumerOwner,
                    HeatingArea = model.HeatingArea,
                    HouseId = model.HouseId,
                    NumberOfPersons = model.NumberOfPersons,
                    ConsumerEmail = model.ConsumerEmail
                    // Інші властивості, якщо вони є
                };

                // Викликайте службу для збереження об'єкта в базі даних
                await _consumerService.AddConsumer(consumer);

                // Після успішного збереження перенаправте користувача на сторінку з індексом
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Якщо виникає помилка, додайте повідомлення про помилку до ModelState
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            // Якщо ModelState не є валідним, поверніть сторінку з формою знову разом з валідаційними помилками
           return RedirectToAction(nameof(Index));
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
