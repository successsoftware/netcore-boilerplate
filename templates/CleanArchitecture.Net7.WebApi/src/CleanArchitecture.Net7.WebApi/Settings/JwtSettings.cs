namespace CleanArchitecture.Net7.WebApi.Settings
{
    public class JwtSettings
    {
        public const string Name = "JWT";
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}
