namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ForgotPasswordEndpoint : Endpoint<ForgotPasswordRequest, ForgotPasswordResponse>
    {
        private readonly IUserService _userService;

        public ForgotPasswordEndpoint(IUserService userService)
        {
            _userService = userService;
        }

        public override void Configure()
        {
            Post("/user/auth/forgot-password");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ForgotPasswordRequest r, CancellationToken c)
        {
            var result = await _userService.ForgotPasswordAsync(r);

            if (result is null)
                await SendNotFoundAsync(c);

            if (result.IsError)
                ThrowError(result.Message);

            await SendNoContentAsync(c);
        }
    }
}