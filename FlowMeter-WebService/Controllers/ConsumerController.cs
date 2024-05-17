namespace FlowMeter_WebService.Controllers
{
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Application.ViewModels;
    using Infrastructure.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Authorize]
    public class ConsumerController : Controller
    {
        private IConsumerService _consumerService;
        private IAccountService _accountService;
        private IHouseService _houseService;
        private readonly ILogger<ConsumerController> _logger;
        private IUserService _userService;

        public ConsumerController(IConsumerService service, IAccountService account, IHouseService houseService, ILogger<ConsumerController> logger, IUserService userService)
        {
            _consumerService = service;
            _accountService = account;
            _houseService = houseService;
            _logger = logger;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Create(Consumer model, string houseAddress)
        {
            var houseModel = await _houseService.GetHouseByAddress(houseAddress);
            ModelState.Remove("House");
            model.HouseId = houseModel.HouseId;
            if (!ModelState.IsValid)
            {
                ViewBag.ShowModalCreate = true;
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"Validation error: {error.ErrorMessage}");
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await _consumerService.CreateConsumer(model, houseAddress);

            var accountModel = new Account{ PersonalAccount = model.PersonalAccount };
            await _accountService.CreateAccount(accountModel);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ConsumerUpdateViewModel consumer)
        {
            if (ModelState.IsValid)
            {
				ViewBag.ShowModalUpdate = true;
				var updatedConsumer = await _consumerService.UpdateConsumer(consumer);
                TempData["message"] = "The consumer has been edited successfuly";
                return RedirectToAction(nameof(Index));
            }

            TempData["message"] = "The consumer has not been edited successfuly";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var deletedConsumer = await _consumerService.DeleteConsumer(id);
            if (deletedConsumer == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Consumer with PersonalAccount: {PersonalAccount} deleted successfully", deletedConsumer.PersonalAccount);
            return RedirectToAction(nameof(Index));
        }
    }
}