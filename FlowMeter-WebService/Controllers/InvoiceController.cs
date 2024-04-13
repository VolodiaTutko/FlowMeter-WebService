using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowMeter_WebService.Controllers
{
    public class InvoiceController : Controller
    {

        private IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService service, ILogger<InvoiceController> logger)
        {
            _invoiceService = service;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
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

            var fileBytes = receipt.PDF;
            var fileName = $"invoice_{receipt.PersonalAccount}_{receipt.Date.ToString("yyyyMMdd")}.pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
