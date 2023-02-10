using Microsoft.Extensions.DependencyInjection;

namespace CoreApiTemplate.TestKit.Extensions
{
    public static class TestKitExtension
    {
        public static IServiceCollection Override<TSource>(this IServiceCollection services,
            Func<IServiceCollection, IServiceCollection> register)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(TSource));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            register(services);

            return services;
        }
    }
}
