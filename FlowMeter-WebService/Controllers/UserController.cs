using Application.Services;
using Application.Services.Interfaces;
using FlowMeter_WebService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlowMeter_WebService.ViewModels;

namespace FlowMeter_WebService.Controllers
{
    public class UserController : Controller
    {
        private IConsumerService _consumerService;
        private IHouseService _houseService;
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<UserController> _logger;

        public UserController(IConsumerService consumerService, IHouseService houseService, IInvoiceService invoiceService, ILogger<UserController> logger)
        {
            _consumerService = consumerService;
            _houseService = houseService;
            _invoiceService = invoiceService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string personalAccountToFind = "2343252333";

            var consumer = await _consumerService.GetConsumerByPersonalAccount(personalAccountToFind);

            if (consumer != null)
            {
                var house = await _houseService.GetHouseById(consumer.HouseId);
                ViewBag.HouseAddress = house?.HouseAddress;

                // Fetch receipts for the consumer
                var receipts = await _invoiceService.GetInvoiceByPersonalAccount(personalAccountToFind);

                var viewModel = new ConsumerInvoicesViewModel
                {
                    Consumer = consumer,
                    Receipts = receipts
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