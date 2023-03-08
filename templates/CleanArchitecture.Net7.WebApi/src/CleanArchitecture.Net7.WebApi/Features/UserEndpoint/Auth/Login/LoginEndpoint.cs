using System.Security.Claims;

using FastEndpoints.Security;

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class LoginEndpoint : Endpoint<LoginRequest, TokenResponse>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public LoginEndpoint(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Post("/user/auth/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest r, CancellationToken c)
        {
            var user = await _userManager.FindByEmailAsync(r.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, r.Password))
            {
                if (user.LockoutEnabled)
                    ThrowError("User is locked out!");

                if (!user.EmailConfirmed)
                    ThrowError("User email is not confirmed!");

                var resp = await CreateTokenWith<TokenService>(user.Id, async p =>
                    {
                        p.Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        p.Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        var userRoles = await _userManager.GetRolesAsync(user);

                        foreach (var userRole in userRoles)
                        {
                            p.Claims.Add(new Claim(ClaimTypes.Role, userRole));
                        }
                    });

                await SendOkAsync(resp, c);
            }
            else
            {
                ThrowError("Invalid username or password");
            }
        }
    }
}