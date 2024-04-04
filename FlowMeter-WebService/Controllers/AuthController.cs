using Application.Models;
using Application.Services.Interfaces;
using FlowMeter_WebService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlowMeter_WebService.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConsumerService _consumerService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
 
        public AuthController(ILogger<AuthController> logger, IConsumerService consumerService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _consumerService = consumerService;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignupEmailViewModel signupEmailViewModel)
        {
            var emailUser = await _consumerService.GetConsumerByEmail(signupEmailViewModel.ConsumerEmail);

            if (emailUser is not null)
            {
                return RedirectToAction("EmailVerification", "Auth", signupEmailViewModel);
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult EmailVerification()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailVerification(SignupEmailViewModel signupEmailViewModel, List<int> VerificationCodes)
        {
            List<int> myList = new List<int> { 1, 2, 3, 4, 5 };
            if (signupEmailViewModel is not null && myList.SequenceEqual(VerificationCodes))
            {

                return RedirectToAction("SetPassword", "Auth", signupEmailViewModel);
            }

            return View();
        }

        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SignupEmailViewModel signupEmailViewModel)
        {
            if (signupEmailViewModel.Password != signupEmailViewModel.ReTypePassword)
            {
                ModelState.AddModelError("ReTypePassword", "Passwords do not match.");
                return View(signupEmailViewModel);
            }

            var user = new User()
            {
                ConsumerEmail = signupEmailViewModel.ConsumerEmail,
                UserName = signupEmailViewModel.ConsumerEmail
                
            };

            var result = await _userManager.CreateAsync(user, signupEmailViewModel.Password);
            if (result.Succeeded)
            { 
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            return View(signupEmailViewModel);
        }



        public IActionResult LogInUser()
        {
            return View();
        }

        public IActionResult LogInAdmin()
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
