using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlowMeter_WebService.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        
        

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogInUser()
        {
            
            return View();

        }
        public IActionResult LogInAdmin()
        {
            
            return View();
        }
        public IActionResult SignUp()
        {
            

            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
