using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class SignupRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SuccessReturnUrl { get; set; }
        public string FailedReturnUrl { get; set; }
    }

    public class SignupValidator : Validator<SignupRequest>
    {
        public SignupValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.SuccessReturnUrl).NotNull().NotEmpty();
            RuleFor(x => x.FailedReturnUrl).NotNull().NotEmpty();
        }
    }
}