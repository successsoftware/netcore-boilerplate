using CoreApiTemplate.Application.Mapping;
using CoreApiTemplate.Application.Proxies;
using CoreApiTemplate.Application.Services;
using CoreApiTemplate.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSS.AspNetCore.Extensions;
using SSS.AspNetCore.Extensions.ServiceProfiling;
using SSS.AspNetCore.Extensions.Swagger;
using SSS.AspNetCore.Extensions.Versioning;
using System;

namespace CoreApiTemplate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerConfig(new SwaggerInfo
            {
                Title = "WebApi Sample",
                Version = "v1.0"
            });

            services.AddHttpContextAccessor();

            services.AddMapping();

            services.AddValidation();

            services.AddVersioning();

            return services.AddServices();
        }

        /// <summary>
        /// This uses for demo Mock Server.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddProxies(this IServiceCollection services, IConfiguration configuration)
        {
            var useMockServer = configuration.GetValue<bool>("UseMockServer");

            if (useMockServer)
            {
                services.AddSingleton(PublicApiProxy.GetInstance(MockServer.CreateHttpClient("http://localhost")));
            }
            else
            {
                services.AddHttpClient<PublicApiProxy>(c => c.BaseAddress = new Uri("https://api.publicapis.org"));
            }

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddProfiling<IToDoService, ToDoService>();

            return services;
        }
    }
}
