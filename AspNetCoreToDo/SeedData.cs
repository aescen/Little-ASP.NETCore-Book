using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreToDo{
    public static class SeedData{
        private const string testAdminPassword = "NotSecure123!!!";

        public static async Task InitializeAsync(IServiceProvider services){
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }
        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager){
            var alreadyExist = await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if(alreadyExist) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }
        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager){
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();
            
            if(testAdmin != null) return;

            testAdmin = new IdentityUser{
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            await userManager.CreateAsync(testAdmin, testAdminPassword);
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }
    }
}