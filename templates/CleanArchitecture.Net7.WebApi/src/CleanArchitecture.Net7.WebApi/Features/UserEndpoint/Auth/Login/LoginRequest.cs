using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class LoginRequest : BaseRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequestValidator : Validator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}