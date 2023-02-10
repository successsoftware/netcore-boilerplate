using CoreApiTemplate.TestKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiTemplate.IntegrationTest.Configurations
{
    public abstract class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected abstract string DatabaseName { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddEnvironmentVariables();
            });

            builder.ConfigureTestServices(services =>
            {
                RegisterStub(services);

                services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            });

            base.ConfigureWebHost(builder);
        }

        protected abstract void RegisterStub(IServiceCollection services);
    }
}