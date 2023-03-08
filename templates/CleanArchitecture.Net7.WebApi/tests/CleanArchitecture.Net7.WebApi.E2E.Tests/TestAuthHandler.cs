using System.Security.Claims;
using System.Text.Encodings.Web;

using CleanArchitecture.Net7.WebApi.Data.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, Contants.ADMIN_ID),
                new Claim(ClaimTypes.Name, "Integration Test"),
                new Claim(ClaimTypes.GivenName, "Integration Test"),
                new Claim(ClaimTypes.Role, UserRoles.Admin)
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
