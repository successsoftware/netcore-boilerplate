using System.Net.Http.Headers;

using CleanArchitecture.Net7.WebApi.Configurations;
using CleanArchitecture.Net7.WebApi.Data;
using CleanArchitecture.Net7.WebApi.E2E.Tests.Extensions;
using CleanArchitecture.Net7.WebApi.E2E.Tests.Stubs;

using CleanArchitecuture.Net7.EmailProvider;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Net7.WebApi.E2E.Tests;

public class BaseWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _container;

    public BaseWebApplicationFactory()
    {
        _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                            .WithDatabase(new PostgreSqlTestcontainerConfiguration
                            {
                                Database = "testDb",
                                Username = "postgres",
                                Password = "postgres"
                            })
                            .WithImage("postgres:latest")
                            .WithCleanUp(true)
                            .Build();
    }

    public HttpClient Client
    {
        get
        {

            var client = CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            client.DefaultRequestHeaders.Add("X-Test-Header", "Test");

            return client;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(logging => logging.ClearProviders());

        builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddEnvironmentVariables();
            });

        builder.ConfigureTestServices(services =>
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddTestAuth<TestUserManager>(configuration);

            services.RemoveDbContext<AppIdentityDbContext>();

            services.AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(_container.ConnectionString));

            services.AddStubs();

            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });

        base.ConfigureWebHost(builder);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();

    public TService GetScopeService<TService>()
    {
        var scope = Server.Services.CreateScope();

        return scope.ServiceProvider.GetService<TService>();
    }

    public TService GetService<TService>() => Server.Services.GetRequiredService<TService>();
}

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStubs(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, TestEmailProvider>();
        return services;
    }
}