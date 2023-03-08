using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ConfirmEmailRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string SuccessReturnUrl { get; set; }
        public string FailedReturnUrl { get; set; }
    }

    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Code).NotNull().NotEmpty();
            RuleFor(x => x.SuccessReturnUrl).NotNull().NotEmpty();
            RuleFor(x => x.FailedReturnUrl).NotNull().NotEmpty();
        }
    }
}