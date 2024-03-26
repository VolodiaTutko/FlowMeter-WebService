using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlowMeter_WebService.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogInUser()
        {
            var p = "_UserLogIn.cshtml";
            return View("~/Views/Auth/Index.cshtml", p);
            
        }
        public IActionResult LogInAdmin()
        {
            var p = "_AdminLogIn.cshtml";
            return View("~/Views/Auth/Index.cshtml", p);
        }
        public IActionResult SignUp()
        {
            var p = "_SignUp.cshtml";
            return View("~/Views/Auth/Index.cshtml", p);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
