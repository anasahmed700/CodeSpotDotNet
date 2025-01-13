using CodeSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace CodeSpot.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            await CreateUserWithRole(userManager, "admin@codespot.com", "admin", "Admin@1234", Roles.Admin);
            await CreateUserWithRole(userManager, "employee@codespot.com", "employee", "Employee@1234", Roles.JobSeeker);
            await CreateUserWithRole(userManager, "employer@codespot.com", "employer", "Employer@1234", Roles.Employer);
        }

        private static async Task CreateUserWithRole(UserManager<IdentityUser> userManager, string email, string username, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = username
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed creating user with email {user.Email}. Errors: {string.Join(",", result.Errors)}");
                }
            }
        }
    }
}
