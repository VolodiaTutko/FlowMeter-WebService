using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlowMeter_WebService.Controllers
{
    [Authorize]
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
        [HttpPost]
        public async Task<ActionResult> PdfTest()
        {
            // List < string personalAccount, List < string typeOfAccount,int price,int accrued>>
            List<(string, List<(string, int, int)>)> data = new List<(string, List<(string, int, int)>)>();
            data.Add(("1234567810", new List<(string, int, int)>
            {
                ("Gas", 100, 1000),
                ("ColdWater", 110, 770)
            }));
            var pdfT = await _invoiceService.Add(data);
            
            return View("~/Views/Invoice/Index.cshtml");
        }
    }
}
