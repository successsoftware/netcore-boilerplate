using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecuture.Net7.EmailProvider
{
    public static class DISetup
    {
        public static IServiceCollection AddEmailProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<EmailOptionsSetup>();

            services.AddScoped<IEmailService, SmtpEmailService>();

            return services;
        }
    }
}