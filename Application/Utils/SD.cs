using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class SD
    {
        public const string User = "User";
        public const string Admin = "Admin";

        public static async Task CheckAndCreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Admin));
            }

            if (!await roleManager.RoleExistsAsync(User))
            {
                await roleManager.CreateAsync(new IdentityRole(User));
            }
        }
    }
}
