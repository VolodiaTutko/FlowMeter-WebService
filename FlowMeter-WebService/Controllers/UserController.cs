// <copyright file="UserController.cs" company="FlowMeter">
// Copyright (c) FlowMeter. All rights reserved.
// </copyright>

namespace FlowMeter_WebService.Controllers
{
    using System.Linq;

    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Application.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize]
    public class UserController : Controller
    {
        private readonly IConsumerService consumerService;
        private readonly IHouseService houseService;
        private readonly IServiceService serviceService;
        private readonly IAccountService accountService;
        private readonly IMeterService meterService;
        private readonly IInvoiceService invoiceService;
        private readonly UserManager<User> userManager;
        private readonly ILogger<UserController> logger;

        public UserController(IConsumerService consumerService, IHouseService houseService, IInvoiceService invoiceService, IServiceService serviceService, IAccountService accountService, IMeterService meterService, ILogger<UserController> logger, UserManager<User> userManager)
        {
            this.consumerService = consumerService;
            this.houseService = houseService;
            this.invoiceService = invoiceService;
            this.serviceService = serviceService;
            this.accountService = accountService;
            this.meterService = meterService;
            this.userManager = userManager;
            this.logger = logger;
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (currentUser != null)
            {
                var consumer = await this.consumerService.GetConsumerByEmail(currentUser.ConsumerEmail);

                if (consumer != null)
                {
                    var house = await this.houseService.GetHouseById(consumer.HouseId);
                    this.ViewBag.HouseAddress = house?.HouseAddress;

                    var receipts = await this.invoiceService.GetInvoiceByPersonalAccount(consumer.PersonalAccount);

                    var services = await this.serviceService.GetServiceByHouseId(consumer.HouseId);

                    var serviceTypePrices = new Dictionary<string, int?>();
                    foreach (var service in services.Value)
                    {
                        serviceTypePrices.Add(service.TypeOfAccount, service.Price);
                    }

                    var account = await this.accountService.GetAccountByPerconalAccount(consumer.PersonalAccount);

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
                            var meterInfo = await this.meterService.GetMeterInfoByAccount(acc.Value);
                            if (meterInfo != null)
                            {
                                existingMeters.Add(acc.Key, meterInfo?.CurrentIndicator);
                            }
                            else
                            {
                            existingMeters.Add(acc.Key, 0);
                            }
                        }
                        else
                        {
                            if (services.Value.Any(s => s.TypeOfAccount == acc.Key))
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
            var receipt = await this.invoiceService.GetInvoiceById(id);
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
