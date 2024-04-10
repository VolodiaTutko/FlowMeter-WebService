﻿using Application.Services;
using Application.Services.Interfaces;
using FlowMeter_WebService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlowMeter_WebService.ViewModels;
using Application.Models;
using System.Linq;

namespace FlowMeter_WebService.Controllers
{
    public class UserController : Controller
    {
        private IConsumerService _consumerService;
        private IHouseService _houseService;
        private IServiceService _serviceService;
        private IAccountService _accountService;
        private IMeterService _meterService;
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<UserController> _logger;

        public UserController(IConsumerService consumerService, IHouseService houseService, IInvoiceService invoiceService, IServiceService serviceService, IAccountService accountService, IMeterService meterService, ILogger<UserController> logger)
        {
            _consumerService = consumerService;
            _houseService = houseService;
            _invoiceService = invoiceService;
            _serviceService = serviceService;
            _accountService = accountService;
            _meterService = meterService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string personalAccountToFind = "7894561230";

            var consumer = await _consumerService.GetConsumerByPersonalAccount(personalAccountToFind);

            if (consumer != null)
            {
                var house = await _houseService.GetHouseById(consumer.HouseId);
                ViewBag.HouseAddress = house?.HouseAddress;

                var receipts = await _invoiceService.GetInvoiceByPersonalAccount(personalAccountToFind);

                var services = await _serviceService.GetServiceByHouseId(consumer.HouseId);

                var serviceTypePrices = new Dictionary<string, int?>();
                foreach (var service in services)
                {
                    serviceTypePrices.Add(service.TypeOfAccount, service.Price);
                }

                var account = await _accountService.GetAccountByPerconalAccount(personalAccountToFind);

                var hotWaterCounterAccount = account.HotWater;
                var coldWaterCounterAccount = account.ColdWater;
                var electricityCounterAccount = account.Electricity;
                var heatingCounterAccount = account.Heating;
                var gasCounterAccount = account.Gas;

                var hotWaterMeter = await _meterService.GetMeterByCounterAccount(hotWaterCounterAccount);
                var coldWaterMeter = await _meterService.GetMeterByCounterAccount(coldWaterCounterAccount);
                var electricityMeter = await _meterService.GetMeterByCounterAccount(electricityCounterAccount);
                var heatingMeter = await _meterService.GetMeterByCounterAccount(heatingCounterAccount);
                var gasMeter = await _meterService.GetMeterByCounterAccount(gasCounterAccount);

                var meters = new Dictionary<string, decimal?>
                {
                    //{ hotWaterMeter?.TypeOfAccount.ToString(), hotWaterMeter?.CurrentIndicator },
                    //{ coldWaterMeter?.TypeOfAccount.ToString(), coldWaterMeter?.CurrentIndicator },
                    //{ electricityMeter?.TypeOfAccount.ToString(), electricityMeter?.CurrentIndicator },
                    //{ heatingMeter?.TypeOfAccount.ToString(), heatingMeter?.CurrentIndicator },
                    //{ gasMeter?.TypeOfAccount.ToString(), gasMeter?.CurrentIndicator }
                };

                var viewModel = new ConsumerInvoicesViewModel
                {
                    Consumer = consumer,
                    Receipts = receipts,
                    ServiceType = serviceTypePrices,
                    Meters = meters
                };

                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<ActionResult> Download(int id)
        {
            var receipt = await _invoiceService.GetInvoiceById(id);
            if (receipt == null)
            {
                return NotFound();
            }

            try
            {
                var fileBytes = receipt.PDF;
                var fileName = $"invoice_{receipt.PersonalAccount}_{receipt.Date.ToString("yyyyMMdd")}.pdf";
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading the invoice.");
                return StatusCode(500);
            }
        }
    }
}