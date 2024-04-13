using Application.Models;
using Microsoft.AspNetCore.Authentication;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateAdminAsync()
        {
            var userExist = await _userManager.FindByEmailAsync("Flowmeter@gamil.com");
            if (userExist == null)
            {
                var user = new User()
                {
                    ConsumerEmail = "Flowmeter@gamil.com",
                    Email = "Flowmeter@gamil.com",
                    UserName = "Flowmeter@gamil.com",
                };

                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, "Fm123456@");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.Admin);
                }
            }
        }
    }
}
