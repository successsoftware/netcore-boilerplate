using System.Web;

using CleanArchitecture.Net7.WebApi.Constants;
using CleanArchitecture.Net7.WebApi.Data.Models;
using CleanArchitecture.Net7.WebApi.Features.UserEndpoint.Auth.ForgotPassword;

using CleanArchitecuture.Net7.EmailProvider;

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Features.UserEndpoint
{
    public interface IUserService
    {
        Task<ChangePasswordResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<string> VerifyForgotPasswordTokenAsync(VerifyForgotPasswordRequest request);
        Task<SetPasswordResponse> SetPasswordAsync(SetPasswordRequest request);
        Task<string> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<ResendEmailConfirmResponse> ResendConfirmationEmailAsync(ResendEmailConfirmRequest request);
        Task<SignupResponse> CreateAsync(SignupRequest request);
    }

    public class UserService : BaseService, IUserService
    {
        private const string Url = "http://localhost:5000";

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignupMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager,
            IServiceResolver serviceResolver,
            IEmailService emailService)
            : base(httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = (SignupMapper)serviceResolver.CreateSingleton(typeof(SignupMapper));
            _emailService = emailService;
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return null;

            var identityResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            return !identityResult.Succeeded
                ? (new() { Message = identityResult.Errors.FirstOrDefault()?.Description, IsError = true })
                : (new());
        }

        public async Task<string> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return $"{request.FailedReturnUrl}";

            var result = await _userManager.ConfirmEmailAsync(user, request.Code);

            return !result.Succeeded ? $"{request.FailedReturnUrl}" : request.SuccessReturnUrl;
        }

        public async Task<SignupResponse> CreateAsync(SignupRequest request)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);

            if (userExist is not null)
                return new() { Message = "User already exists!", IsError = true };

            var user = _mapper.ToEntity(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new() { Message = result.Errors.FirstOrDefault()?.Description, IsError = true };

            result = await _userManager.AddToRoleAsync(user, UserRoles.User);

            if (!result.Succeeded)
                return new() { Message = result.Errors.FirstOrDefault()?.Description, IsError = true };

            await SendEmailConfirmAsync(user, request.SuccessReturnUrl, request.FailedReturnUrl);

            return new();
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return null;

            if (!user.EmailConfirmed)
                return new() { Message = "User email is not confirmed!", IsError = true };

            if (user.LockoutEnabled)
                return new() { Message = "User is not activated!", IsError = true };

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = HttpUtility.UrlEncode(code);

            var returnUrl = string.Format(ReturnUrls.EMAIL_FORGOTPASSWORD_URL, Url, $"code={code}&email={user.Email}&successReturnUrl={request.SuccessReturnUrl}&failedReturnUrl={request.FailedReturnUrl}");

            var message = $$"""
                    <p>Please click the below link to reset your password:</p>
                    <p><a href="{{returnUrl}}">{{returnUrl}}</a></p>
                    """;

            await _emailService.SendAsync(user.Email, "Reset Password", message);

            return new();
        }

        public async Task<ResendEmailConfirmResponse> ResendConfirmationEmailAsync(ResendEmailConfirmRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return null;

            await SendEmailConfirmAsync(user, request.SuccessReturnUrl, request.FailedReturnUrl);

            return new();
        }

        public async Task<SetPasswordResponse> SetPasswordAsync(SetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return null;

            var identityResult = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);

            return !identityResult.Succeeded
                ? (new() { Message = identityResult.Errors.FirstOrDefault()?.Description, IsError = true })
                : (new());
        }

        public async Task<string> VerifyForgotPasswordTokenAsync(VerifyForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var identityResult = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", request.Code);

            return !identityResult
                ? request.FailedReturnUrl
                : $"{request.SuccessReturnUrl}?code={HttpUtility.UrlEncode(request.Code)}&email={request.Email}";
        }

        #region PRIVATE
        private async Task SendEmailConfirmAsync(IdentityUser user, string successReturnUrl, string failedReturnUrl)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = HttpUtility.UrlEncode(code);

            var returnUrl = string.Format(ReturnUrls.EMAIL_CONFIRMATION_URL, Url, $"code={code}&email={user.Email}&successReturnUrl={successReturnUrl}&failedReturnUrl={failedReturnUrl}");

            var message = $$"""
                    <p>Please click the below link to verify your email address:</p>
                    <p><a href="{{returnUrl}}">{{returnUrl}}</a></p>
                    """;

            await _emailService.SendAsync(user.Email, "Confirm your email", message);
        }
        #endregion
    }
}
