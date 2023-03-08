using NetCore.AutoRegisterDi;

namespace CleanArchitecture.Net7.WebApi.Extensions
{
    public static class DISetupExtensions
    {
        public static IServiceCollection RegisterServiceLayerDi(this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(x => x.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            return services;
        }
    }
}
