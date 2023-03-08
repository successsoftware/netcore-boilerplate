using CleanArchitecture.Net7.WebApi.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests.Stubs
{
    public class TestUserManager : UserManager<IdentityUser>
    {
        public TestUserManager(IUserStore<IdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<TestUserManager> logger)
            : base(store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
        }

        public override async Task<IdentityUser> FindByEmailAsync(string email)
        {
            return email != "admin"
                ? null
                : await Task.FromResult(new IdentityUser
                {
                    Id = Contants.ADMIN_ID,
                    UserName = "admin",
                    Email = "admin",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                });
        }

        public override async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return user.Email == "admin" && password == "admin@123"
                ? await Task.FromResult(true)
                : await Task.FromResult(false);
        }

        public override async Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            return user.Email == "admin"
                ? await Task.FromResult(new List<string> { UserRoles.Admin })
                : new();

        }
    }
}
