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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
 
        public AuthController(ILogger<AuthController> logger, IConsumerService consumerService, SignInManager<User> signInManager, UserManager<User> userManager)
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
        public async Task<IActionResult> EmailVerification(SignupEmailViewModel signupEmailViewModel, string[] VerificationCodes)
        {
            string concatenatedCodes = string.Join("", VerificationCodes);
            string validationCode = "12345";
            if (signupEmailViewModel is not null && validationCode == concatenatedCodes)
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
                Email = signupEmailViewModel.ConsumerEmail,
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

        [HttpGet]
        public IActionResult LogInUser()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInUser(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.ConsumerEmail, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(loginViewModel);
            }
        }

        public IActionResult LogInAdmin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
