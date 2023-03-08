using System.Security.Claims;

using CleanArchitecture.Net7.WebApi.Data.Models;
using CleanArchitecture.Net7.WebApi.Interfaces;
using CleanArchitecture.Net7.WebApi.Settings;

using FastEndpoints.Security;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class TokenService : RefreshTokenService<TokenRequest, TokenResponse>
    {
        public readonly IAppDbContext _dbContext;

        public TokenService(JwtSettings jwtSettings, IAppDbContext dbContext)
        {
            Setup(x =>
            {
                x.TokenSigningKey = jwtSettings.Secret;
                x.AccessTokenValidity = TimeSpan.FromMinutes(1);
                x.RefreshTokenValidity = TimeSpan.FromHours(1);
                x.Endpoint("/user/auth/refresh-token", ep =>
                {
                    ep.Summary(s => s.Description = "this is the refresh token endpoint");
                });
            });
            _dbContext = dbContext;
        }

        public override async Task PersistTokenAsync(TokenResponse rsp)
        {
            var refreshToken = await _dbContext.Set<RefreshToken>().SingleOrDefaultAsync(x => x.UserId.Equals(rsp.UserId));

            if (refreshToken is not null)
            {
                _dbContext.Set<RefreshToken>().Remove(refreshToken);
            }

            _dbContext.Set<RefreshToken>().Add(new RefreshToken
            {
                UserId = rsp.UserId,
                ExpiryDate = rsp.RefreshExpiry,
                Token = rsp.RefreshToken
            });

            await _dbContext.SaveChangesAsync();
        }

        public override async Task RefreshRequestValidationAsync(TokenRequest req)
        {
            var refreshToken = await _dbContext.Set<RefreshToken>().SingleOrDefaultAsync(x => x.UserId.Equals(req.UserId)
                && x.Token.Equals(req.RefreshToken)
                && x.ExpiryDate > DateTime.UtcNow);

            if (refreshToken is null)
                ThrowError("Refresh token is invalid!");
        }

        public override Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
        {
            privileges.Claims.Add(new(ClaimTypes.NameIdentifier, request.UserId));
            return Task.CompletedTask;
        }
    }
}