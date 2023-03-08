namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class VerifyForgotPasswordEndpoint : Endpoint<VerifyForgotPasswordRequest>
    {
        private readonly IUserService _userService;

        public VerifyForgotPasswordEndpoint(IUserService userService)
        {
            _userService = userService;
        }

        public override void Configure()
        {
            Get("/user/auth/verify-forgot-password");
            AllowAnonymous();
        }

        public override async Task HandleAsync(VerifyForgotPasswordRequest r, CancellationToken c)
        {
            var result = await _userService.VerifyForgotPasswordTokenAsync(r);

            if (result is null)
                await SendNotFoundAsync(c);

            await SendRedirectAsync(result, cancellation: c);
        }
    }
}