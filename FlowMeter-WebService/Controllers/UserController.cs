using Application.Services;
using Application.Services.Interfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FlowMeter_WebService.Controllers
{
    public class UserController : Controller
    {
        private readonly IConsumerService _consumerService;
        private readonly IHouseService _houseService;
        private readonly IServiceService _serviceService;
        private readonly IAccountService _accountService;
        private readonly IMeterService _meterService;
        private readonly IInvoiceService _invoiceService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IConsumerService consumerService, IHouseService houseService, IInvoiceService invoiceService, IServiceService serviceService, IAccountService accountService, IMeterService meterService, ILogger<UserController> logger, UserManager<User> userManager)
        {
            this._consumerService = consumerService;
            this._houseService = houseService;
            this._invoiceService = invoiceService;
            this._serviceService = serviceService;
            this._accountService = accountService;
            this._meterService = meterService;
            this._userManager = userManager;
            this._logger = logger;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await this._userManager.GetUserAsync(this.User);
            if (currentUser != null)
            {
                var consumer = await this._consumerService.GetConsumerByEmail(currentUser.ConsumerEmail);


                if (consumer != null)
                {
                    var house = await this._houseService.GetHouseById(consumer.HouseId);
                    this.ViewBag.HouseAddress = house?.HouseAddress;

                    var receipts = await this._invoiceService.GetInvoiceByPersonalAccount(consumer.PersonalAccount);

                    var services = await this._serviceService.GetServiceByHouseId(consumer.HouseId);

                    var serviceTypePrices = new Dictionary<string, int?>();
                    foreach (var service in services)
                    {
                        serviceTypePrices.Add(service.TypeOfAccount, service.Price);
                    }

                    var account = await this._accountService.GetAccountByPerconalAccount(consumer.PersonalAccount);

                    var hotWaterCounterAccount = account.HotWater;
                    var coldWaterCounterAccount = account.ColdWater;
                    var electricityCounterAccount = account.Electricity;
                    var heatingCounterAccount = account.Heating;
                    var gasCounterAccount = account.Gas;

                    var hotWaterMeter = await this._meterService.GetMeterByCounterAccount(hotWaterCounterAccount);
                    var coldWaterMeter = await this._meterService.GetMeterByCounterAccount(coldWaterCounterAccount);
                    var electricityMeter = await this._meterService.GetMeterByCounterAccount(electricityCounterAccount);
                    var heatingMeter = await this._meterService.GetMeterByCounterAccount(heatingCounterAccount);
                    var gasMeter = await this._meterService.GetMeterByCounterAccount(gasCounterAccount);

                    var hotWaterMeterRecords = await this._meterService.GetMeterById(hotWaterMeter.MeterId);
                    var coldWaterMeterRecords = await this._meterService.GetMeterById(coldWaterMeter.MeterId);
                    var electricityMeterRecords = await this._meterService.GetMeterById(electricityMeter.MeterId);
                    var heatingMeterRecords = await this._meterService.GetMeterById(heatingMeter.MeterId);
                    var gasMeterRecords = await this._meterService.GetMeterById(gasMeter.MeterId);

                    var meters = new Dictionary<string, decimal?>
                    {
                        { hotWaterMeter?.TypeOfAccount.ToString(), hotWaterMeterRecords?.CurrentIndicator },
                        { coldWaterMeter?.TypeOfAccount.ToString(), coldWaterMeterRecords?.CurrentIndicator },
                        { electricityMeter?.TypeOfAccount.ToString(), electricityMeterRecords?.CurrentIndicator },
                        { heatingMeter?.TypeOfAccount.ToString(), heatingMeterRecords?.CurrentIndicator },
                        { gasMeter?.TypeOfAccount.ToString(), gasMeterRecords?.CurrentIndicator },
                    };

                    var viewModel = new ConsumerInvoicesViewModel
                    {
                        Consumer = consumer,
                        Receipts = receipts,
                        ServiceType = serviceTypePrices,
                        Meters = meters,
                    };

                    return this.View(viewModel);
                }
                else
                {
                    return this.NotFound();
                }
            }

            return this.View("Error");
        }

        public async Task<ActionResult> Download(int id)
        {
            var receipt = await this._invoiceService.GetInvoiceById(id);
            if (receipt == null)
            {
                return this.NotFound();
            }

            var fileBytes = receipt.PDF;
            var fileName = $"invoice_{receipt.PersonalAccount}_{receipt.Date.ToString("yyyyMMdd")}.pdf";
            return this.File(fileBytes, "application/pdf", fileName);
        }
    }
}