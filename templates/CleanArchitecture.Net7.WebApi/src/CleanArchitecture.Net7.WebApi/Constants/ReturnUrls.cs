namespace CleanArchitecture.Net7.WebApi.Constants
{
    public static class ReturnUrls
    {
        public const string EMAIL_CONFIRMATION_URL = "{0}/api/user/auth/confirm-email?{1}";
        public const string EMAIL_FORGOTPASSWORD_URL = "{0}/api/user/auth/verify-forgot-password?{1}";
    }
}
