using System.Security.Claims;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest>
    {
        private readonly IUserService _userService;

        public ChangePasswordEndpoint(IUserService userService)
        {
            _userService = userService;
        }

        public override void Configure()
        {
            Put("/user/auth/change-password");
        }

        public override async Task HandleAsync(ChangePasswordRequest r, CancellationToken c)
        {
            var result = await _userService.ChangePasswordAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), r);

            if (result is null)
                await SendNotFoundAsync(c);

            if (result.IsError)
                ThrowError(result.Message);

            await SendNoContentAsync(c);
        }
    }
}