namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ResendEmailConfirmEndpoint : Endpoint<ResendEmailConfirmRequest, ResendEmailConfirmResponse>
    {
        private readonly IUserService _userService;

        public ResendEmailConfirmEndpoint(IUserService userService)
        {
            _userService = userService;
        }

        public override void Configure()
        {
            Get("/user/auth/resend-email-confirmation");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ResendEmailConfirmRequest r, CancellationToken c)
        {
            var result = await _userService.ResendConfirmationEmailAsync(r);

            if (result is null)
                await SendNotFoundAsync(c);

            if (result.IsError)
                ThrowError(result.Message);

            await SendNoContentAsync(c);
        }
    }
}