namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ConfirmEmailEndpoint : Endpoint<ConfirmEmailRequest>
    {
        private readonly IUserService _userService;

        public ConfirmEmailEndpoint(IUserService userService)
        {
            _userService = userService;
        }

        public override void Configure()
        {
            Get("/user/auth/confirm-email");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ConfirmEmailRequest r, CancellationToken c)
        {
            var result = await _userService.ConfirmEmailAsync(r);

            await SendRedirectAsync(result, cancellation: c);
        }
    }
}