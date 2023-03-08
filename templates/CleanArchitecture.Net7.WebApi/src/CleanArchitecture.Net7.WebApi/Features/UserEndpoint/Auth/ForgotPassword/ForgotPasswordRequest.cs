using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ForgotPasswordRequest : BaseRequest
    {
        public string Email { get; set; }
        public string SuccessReturnUrl { get; set; }
        public string FailedReturnUrl { get; set; }
    }

    public class ForgotPasswordValidator : Validator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.SuccessReturnUrl).NotEmpty().NotNull();
            RuleFor(x => x.FailedReturnUrl).NotEmpty().NotNull();
        }
    }
}