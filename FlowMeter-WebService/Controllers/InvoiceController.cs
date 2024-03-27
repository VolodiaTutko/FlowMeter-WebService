using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowMeter_WebService.Controllers
{
    public class InvoiceController : Controller
    {

        private IInvoiceService _invoiceService;
        private readonly ILogger<ConsumerController> _logger;

        public InvoiceController(IInvoiceService service, ILogger<ConsumerController> logger)
        {
            _invoiceService = service;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var invoice = await _invoiceService.GetList();
            return View(invoice);
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
