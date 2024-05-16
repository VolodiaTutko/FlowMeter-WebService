using Application.Models;
using Application.Services.Interfaces;
using Application.Utils;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;

namespace FlowMeter_WebService.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IConsumerService _consumerService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IEmailSenderService _emailSenderService;
 
        public AuthController(ILogger<AuthController> logger, IAuthService authService, IConsumerService consumerService, SignInManager<User> signInManager,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _consumerService = consumerService;
            _authService = authService;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            //_emailSenderService = emailSenderService;
            _roleManager = roleManager;

            Task.Run(async () => await SD.CheckAndCreateRoles(_roleManager)).Wait();
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignupEmailViewModel signupEmailViewModel)
        {
            if (signupEmailViewModel.ConsumerEmail == null)
            {
                return View(signupEmailViewModel);
            }

            var emailUser = await _consumerService.GetConsumerByEmail(signupEmailViewModel.ConsumerEmail);
            var registeredUser = await _userManager.FindByEmailAsync(signupEmailViewModel.ConsumerEmail);
            //signupEmailViewModel.ValidationCode = await _emailSenderService.SendVerificationCode(signupEmailViewModel.ConsumerEmail);

            if (registeredUser != null)
            {
                ModelState.AddModelError(nameof(signupEmailViewModel.ConsumerEmail), "User with this email address already exists");
                return View(signupEmailViewModel);
            }

            if (emailUser is not null)
            {
                TempData["ConsumerEmail"] = signupEmailViewModel.ConsumerEmail;
                TempData["ValidationCode"] = signupEmailViewModel.ValidationCode;
                return RedirectToAction("EmailVerification", "Auth");
            }

            ModelState.AddModelError(nameof(signupEmailViewModel.ConsumerEmail), "No user with this email address was found. Please contact the administrator");
            return View(signupEmailViewModel);
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
        public async Task<IActionResult> EmailVerification([FromForm] SignupEmailViewModel signupEmailViewModel, string[] VerificationCodes)
        {
            string concatenatedCodes = string.Join("", VerificationCodes);
            string validationCode = signupEmailViewModel.ValidationCode;

            if (signupEmailViewModel != null && validationCode == concatenatedCodes)
            {
                TempData["ConsumerEmail"] = signupEmailViewModel.ConsumerEmail;
                return RedirectToAction("SetPassword", "Auth");
            }
            else
            {
                ModelState.AddModelError(nameof(signupEmailViewModel.ValidationCode), "Invalid verification code.");
                return View(signupEmailViewModel);
            }
        }

        [HttpGet]
        public IActionResult SetPassword()
        {
            string consumerEmail = TempData["ConsumerEmail"] as string;
            if (consumerEmail == null)
            {
                return RedirectToAction("SignUp");
            }

            var viewModel = new SignupEmailViewModel
            {
                ConsumerEmail = consumerEmail
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword([FromForm]SignupEmailViewModel signupEmailViewModel)
        {
            if (string.IsNullOrEmpty(signupEmailViewModel.Password) && string.IsNullOrEmpty(signupEmailViewModel.ReTypePassword))
            {
                return View(signupEmailViewModel);
            }

            if (signupEmailViewModel.Password != signupEmailViewModel.ReTypePassword)
            {
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
                await _userManager.AddToRoleAsync(user, SD.User); 
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            return View(signupEmailViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogInUser(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInUser(LoginViewModel loginViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.ConsumerEmail);
                if (user != null)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    if (role.Contains(SD.User))
                    {
                        var result = await _signInManager.PasswordSignInAsync(loginViewModel.ConsumerEmail, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            return LocalRedirect(returnUrl);
                        }

                        if (result.IsLockedOut)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(loginViewModel.Password), "Invalid email or password.");
                            return View(loginViewModel);
                        }
                    }
                }
            }

            ModelState.AddModelError(nameof(loginViewModel.Password), "Invalid role attempt");
            return View(loginViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LogInAdmin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await _authService.CreateAdminAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogInAdmin(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (loginViewModel.ConsumerEmail == null || loginViewModel.Password == null)
            {
                return View(loginViewModel);
            }

            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            var user = await _userManager.FindByEmailAsync(loginViewModel.ConsumerEmail);
            var role = await _userManager.GetRolesAsync(user);

            if (role[0] == SD.Admin)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.ConsumerEmail, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(nameof(loginViewModel.Password), "Invalid email or password.");
                    return View(loginViewModel);
                }
            }

            ModelState.AddModelError(nameof(loginViewModel.Password), "Invalid role attempt");
            return View(loginViewModel);
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
            if (model.ConsumerEmail == null)
            {
                return View(model);
            }

            var registeredUser = await _userManager.FindByEmailAsync(model.ConsumerEmail);
            if (registeredUser == null)
            {
                ModelState.AddModelError(nameof(model.ConsumerEmail), "No user with this email address was found.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(registeredUser);
                var callbackurl = Url.Action("ResetPassword", "Auth", new { userId = registeredUser.Id, code, registeredUser.ConsumerEmail }, protocol:HttpContext.Request.Scheme);

                //await _emailSenderService.SendEmailAsync(model.ConsumerEmail, "Reset Password", 
                //    $"Please reset your password by clicking here: <a href='{callbackurl}'>link</a>");

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
            if (string.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.ReTypePassword))
            {
                return View(model);
            }

            if (model.Password != model.ReTypePassword)
            {
                return View(model);
            }

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
