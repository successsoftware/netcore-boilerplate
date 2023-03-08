using System.Security.Claims;

using CleanArchitecture.Net7.WebApi.Data.Models;
using CleanArchitecture.Net7.WebApi.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class RevokeTokenEndpoint : EndpointWithoutRequest
    {
        private readonly IAppDbContext _dbContext;

        public RevokeTokenEndpoint(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Configure()
        {
            Post("/user/auth/revoke-token");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var refreshToken = await _dbContext.Set<RefreshToken>().SingleOrDefaultAsync(x => x.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            _dbContext.Set<RefreshToken>().Remove(refreshToken);

            await SendNoContentAsync(ct);
        }
    }
}