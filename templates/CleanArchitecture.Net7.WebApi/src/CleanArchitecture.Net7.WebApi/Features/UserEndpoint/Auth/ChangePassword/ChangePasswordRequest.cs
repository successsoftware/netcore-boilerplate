using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class ChangePasswordRequest : BaseRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }

    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotNull().NotEmpty();
            RuleFor(x => x.NewPassword).NotNull().NotEmpty();
            RuleFor(x => x.ReNewPassword).NotNull().NotEmpty().Equal(x => x.NewPassword);
        }
    }
}