using CleanArchitecture.Net7.WebApi.Data;
using CleanArchitecture.Net7.WebApi.Settings;

using FastEndpoints.Security;

using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Net7.WebApi.Configurations
{
    public static class ConfigAuth
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSettings jwtSettings = new();

            configuration.GetSection(JwtSettings.Name).Bind(jwtSettings);

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthenticationJWTBearer(jwtSettings.Secret);

            services.AddSingleton(sp =>
            {
                return jwtSettings;
            });

            return services;
        }

        public static IServiceCollection AddTestAuth<TUserManager>(this IServiceCollection services, IConfiguration configuration)
            where TUserManager : class
        {
            JwtSettings jwtSettings = new();

            configuration.GetSection(JwtSettings.Name).Bind(jwtSettings);

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddUserManager<TUserManager>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthenticationJWTBearer(jwtSettings.Secret);

            services.AddSingleton(sp =>
            {
                return jwtSettings;
            });

            return services;
        }
    }
}
