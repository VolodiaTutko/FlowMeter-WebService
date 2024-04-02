using Microsoft.AspNetCore.Mvc;

namespace FlowMeter_WebService.Controllers
{
    public class PaymentResultsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
