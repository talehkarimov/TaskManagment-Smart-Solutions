using Microsoft.AspNetCore.Identity;
using TaskManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagment.Data
{
    public class AppDbInitializer
    {
        private const string DefaultRole = "Admin";
        private const string DefaultUserName = "Administrator";
        private const string DefaultEmail = "admin@admin.com";
        private const string DefaultPassword = "admin123";



        public static async System.Threading.Tasks.Task SeedDefaultUserAndRoleAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            IdentityResult roleCreated;
            if (await roleManager.FindByNameAsync(DefaultRole) == null)
            {
                var defaultRole = new ApplicationRole
                {
                    Name = DefaultRole
                };
                roleCreated = await roleManager.CreateAsync(defaultRole);
            }
            else
            {
                roleCreated = IdentityResult.Success;
            }

            if (roleCreated.Succeeded)
            {
                if (await userManager.FindByEmailAsync(DefaultEmail) == null)
                {
                    var defaultUser = new ApplicationUser()
                    {
                        UserName = DefaultUserName,
                        Email = DefaultEmail
                    };
                    var result = await userManager.CreateAsync(defaultUser, DefaultPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser, DefaultRole);
                    }
                }
            }
        }

        public static async Task<IdentityResult> SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            IdentityResult result = IdentityResult.Failed();

            foreach (string role in SystemRoles.AllRoles)
            {
                if (await roleManager.FindByNameAsync(role) != null) continue;
                result = await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });
            }
            return result;
        }
    }
}
