using Application.Models;
using Application.Services.Interfaces;
using FlowMeter_WebService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSenderService _emailSenderService;
 
        public AuthController(ILogger<AuthController> logger, IConsumerService consumerService, SignInManager<User> signInManager, UserManager<User> userManager, IEmailSenderService emailSenderService)
        {
            _consumerService = consumerService;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
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
            signupEmailViewModel.ValidationCode = await _emailSenderService.SendVerificationCode(signupEmailViewModel.ConsumerEmail);

            if (emailUser is not null)
            {
                TempData["ConsumerEmail"] = signupEmailViewModel.ConsumerEmail;
                TempData["ValidationCode"] = signupEmailViewModel.ValidationCode;
                return RedirectToAction("EmailVerification", "Auth");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult EmailVerification()
        {
            string consumerEmail = TempData["ConsumerEmail"] as string;
            string validationCode = TempData["ValidationCode"] as string;

            if (consumerEmail == null || validationCode == null)
            {
                return RedirectToAction("SignUp");
            }

            var viewModel = new SignupEmailViewModel
            {
                ConsumerEmail = consumerEmail,
                ValidationCode = validationCode
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailVerification([FromForm]SignupEmailViewModel signupEmailViewModel, string[] VerificationCodes)
        {
            string concatenatedCodes = string.Join("", VerificationCodes);
            string validationCode = signupEmailViewModel.ValidationCode;
            if (signupEmailViewModel is not null && validationCode == concatenatedCodes)
            {
                return RedirectToAction("SetPassword", "Auth", new { ConsumerEmail = signupEmailViewModel.ConsumerEmail, ValidationCode = validationCode });
            }

            return View(signupEmailViewModel);
        }

        [HttpGet]
        public IActionResult SetPassword(string consumerEmail, string validationCode)
        {
            return View(new SignupEmailViewModel { ConsumerEmail = consumerEmail, ValidationCode = validationCode });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword([FromForm]SignupEmailViewModel signupEmailViewModel)
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
                UserName = signupEmailViewModel.ConsumerEmail,
            };

            user.EmailConfirmed = true;
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

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.ConsumerEmail);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code, user.ConsumerEmail }, protocol:HttpContext.Request.Scheme);

                await _emailSenderService.SendEmailAsync(model.ConsumerEmail, "Reset Password", 
                    $"Please reset your password by clicking here: <a href='{callbackurl}'>link</a>");

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string? code = null, string? consumerEmail = null)
        {
            if (code == null)
            {
                return View("Error");
            }

            var model = new ResetPasswordViewModel
            {
                Code = code,
                ConsumerEmail = consumerEmail,
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.ConsumerEmail);
                if (user == null)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
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
