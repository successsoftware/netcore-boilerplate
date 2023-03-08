using CleanArchitecture.Net7.WebApi.Features.Common;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public class SetPasswordRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class SetPasswordRequestValidator : AbstractValidator<SetPasswordRequest>
    {
        public SetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull().Equal(x => x.Password);
        }
    }
}