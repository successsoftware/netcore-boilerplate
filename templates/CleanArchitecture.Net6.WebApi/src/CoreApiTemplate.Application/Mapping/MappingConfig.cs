using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoreApiTemplate.Application.Mapping
{
    public static class MappingConfig
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
