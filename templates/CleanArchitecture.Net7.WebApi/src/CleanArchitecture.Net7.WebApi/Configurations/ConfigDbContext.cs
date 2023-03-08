using CleanArchitecture.Net7.WebApi.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Net7.WebApi.Configurations
{
    public static class ConfigDbContext
    {
        public static IServiceCollection AddSqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext, IAppDbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TContext>(o
                => o.UseSqlServer(connectionString));

            services.AddScoped<IAppDbContext, TContext>();

            return services;
        }

        public static IServiceCollection AddNpgsqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
             where TContext : DbContext, IAppDbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TContext>(o
                => o.UseNpgsql(connectionString));

            services.AddScoped<IAppDbContext, TContext>();

            return services;
        }
    }
}
