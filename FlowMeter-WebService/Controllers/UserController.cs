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
    [Authorize]
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

                    var accounts = new Dictionary<string, string>
                    {
                        { "HotWater", hotWaterCounterAccount },
                        { "ColdWater", coldWaterCounterAccount },
                        { "Electricity", electricityCounterAccount },
                        { "Heating", heatingCounterAccount },
                        { "Gas", gasCounterAccount },
                    };

                    var existingMeters = new Dictionary<string, decimal?>();

                    foreach (var acc in accounts)
                    {
                        if (acc.Value != null)
                        {
                            var meterInfo = await this._meterService.GetMeterInfoByAccount(acc.Value);
                            if (meterInfo != null)
                            {
                                existingMeters.Add(acc.Key, meterInfo?.CurrentIndicator);
                            } else
                            {
                            existingMeters.Add(acc.Key, 0);
                            }
                        } 
                        else
                        {
                            if (services.Any(s => s.TypeOfAccount == acc.Key))
                            {
                                existingMeters.Add(acc.Key, 0);
                            }
                        }
                    }

                    var viewModel = new ConsumerInvoicesViewModel
                    {
                        Consumer = consumer,
                        Receipts = receipts,
                        ServiceType = serviceTypePrices,
                        Meters = existingMeters,
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