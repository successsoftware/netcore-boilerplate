namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class SetPasswordEndpoint : Endpoint<SetPasswordRequest, SetPasswordResponse>
    {
        private readonly IUserService _userService;

        public SetPasswordEndpoint(IUserService userService)
        {
            _userService = userService;
        }

        public override void Configure()
        {
            Post("/user/auth/set-password");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SetPasswordRequest r, CancellationToken c)
        {
            var result = await _userService.SetPasswordAsync(r);

            if (result is null)
                await SendNotFoundAsync(c);

            if (result.IsError)
                ThrowError(result.Message);

            await SendNoContentAsync(c);
        }
    }
}