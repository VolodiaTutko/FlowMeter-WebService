namespace FlowMeter_WebService.Controllers
{
    using Application.Models;
    using Application.Services;
    using Application.Services.Interfaces;
    using Humanizer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [Authorize]
    public class PaymentController : Controller
    {
        private IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService service, ILogger<PaymentController> logger)
        {
            _paymentService = service;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var payment = await _paymentService.GetList();
            return View(payment);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create(Account a)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment model)
        {
            ModelState.Remove("Consumer");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            Random random = new Random();
            var payment = new Payment
            {
                Amount = model.Amount,
                Date = DateTime.UtcNow.Date,
                PersonalAccount = model.PersonalAccount,
                Type = model.Type
            };

            await _paymentService.AddPayment(payment);
            _logger.LogInformation("Payment created successfully with ID: {PaymentId}", payment.PaymentID);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
