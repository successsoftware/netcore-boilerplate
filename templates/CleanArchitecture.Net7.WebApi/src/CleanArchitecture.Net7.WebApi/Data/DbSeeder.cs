using CleanArchitecture.Net7.WebApi.Data.Models;

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Data
{
    public class DbSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRoleASync()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                var role = new IdentityRole
                {
                    Name = UserRoles.Admin
                };
                await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                var role = new IdentityRole
                {
                    Name = UserRoles.User
                };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
