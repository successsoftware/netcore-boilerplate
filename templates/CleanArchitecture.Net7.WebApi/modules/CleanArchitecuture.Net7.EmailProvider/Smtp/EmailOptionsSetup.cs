using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CleanArchitecuture.Net7.EmailProvider
{
    public class EmailOptionsSetup : IConfigureOptions<SmtpSettings>
    {
        private const string ConfigurationSectionName = nameof(SmtpSettings);

        private readonly IConfiguration _configuration;

        public EmailOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(SmtpSettings options)
        {
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
