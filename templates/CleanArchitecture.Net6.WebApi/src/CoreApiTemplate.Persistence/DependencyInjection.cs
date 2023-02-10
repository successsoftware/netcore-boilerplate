using CoreApiTemplate.Application.Interfaces;
using CoreApiTemplate.Persistence.DbContext;
using CoreApiTemplate.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSS.CommonLib.Interfaces;
using SSS.EntityFrameworkCore.Extensions;

namespace CoreApiTemplate.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddAuditContext<TodoContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IAppDbContext, TodoContext>();

            services.AddTransient<SeedData>();

            services.AddTransient<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}
