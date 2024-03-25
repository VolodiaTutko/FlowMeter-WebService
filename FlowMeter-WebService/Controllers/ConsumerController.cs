namespace FlowMeter_WebService.Controllers
{
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ConsumerController : Controller
    {
        private IConsumerService _consumerService;
        private IAccountService _accountService;
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(IConsumerService service, IAccountService account, ILogger<ConsumerController> logger)
        {
            _consumerService = service;
            _accountService = account;
            _logger = logger;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consumer model)
        {
            ModelState.Remove("House");
            ModelState.Remove("User");
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"Validation error: {error.ErrorMessage}");
                    }
                }
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
                _logger.LogInformation("Consumer created successfully with PersonalAccount: {ConsumerId}", consumer.PersonalAccount);

                var account = new Account
                {
                    PersonalAccount = model.PersonalAccount,
                    HotWater = null,
                    ColdWater = null,
                    Heating = null,
                    Electricity = null,
                    Gas = null,
                    PublicService = null
                };

                await _accountService.AddAccount(account);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating consumer");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
            }

            return RedirectToAction(nameof(Index), model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(Consumer updatedConsumer, string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var deletedConsumer = await _consumerService.DeleteConsumer(id);
                if (deletedConsumer == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Consumer with PersonalAccount: {PersonalAccount} deleted successfully", deletedConsumer.PersonalAccount);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting consumer");
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}