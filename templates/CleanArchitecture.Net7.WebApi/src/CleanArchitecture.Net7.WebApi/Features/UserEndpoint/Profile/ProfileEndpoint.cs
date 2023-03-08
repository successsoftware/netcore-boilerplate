using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ProfileEndpoint : EndpointWithoutRequest<ProfileResponse, ProfileMapper>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileEndpoint(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Post("/user/auth/profile");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user is null)
                await SendNotFoundAsync(c);

            await SendOkAsync(Map.FromEntity(user), c);
        }
    }
}