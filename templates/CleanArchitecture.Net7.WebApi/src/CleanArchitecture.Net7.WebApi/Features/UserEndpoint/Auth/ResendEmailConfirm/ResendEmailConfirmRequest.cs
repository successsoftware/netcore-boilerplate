using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ResendEmailConfirmRequest : BaseRequest
    {
        public string Email { get; set; }
        public string SuccessReturnUrl { get; set; }
        public string FailedReturnUrl { get; set; }
    }

    public class ResendEmailConfirmRequestValidator : AbstractValidator<ResendEmailConfirmRequest>
    {
        public ResendEmailConfirmRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.SuccessReturnUrl).NotNull().NotEmpty();
            RuleFor(x => x.FailedReturnUrl).NotNull().NotEmpty();
        }
    }
}