using CoreApiTemplate.IntegrationTest.Configurations;
using CoreApiTemplate.Persistence.DbContext;
using CoreApiTemplate.TestKit.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace CoreApiTemplate.IntegrationTest.Scenarios.ToDo
{
    [CollectionDefinition(Constants.ToDoFixture)]
    public class ToDoFixture : TestWebApplicationFactory<Program>
    {
        public HttpClient Client;

        public ToDoFixture()
        {
            Client = CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            Client.DefaultRequestHeaders.Add("X-Test-Header", "Test");
        }

        protected override string DatabaseName => "ToDoDbTest";

        protected override void RegisterStub(IServiceCollection services)
        {
            var scope = services.BuildServiceProvider().CreateScope();

            var configuration = scope.ServiceProvider.GetService<IConfiguration>();

            var connectString = configuration.GetConnectionString("DefaultConnection");

            services.Override<DbContextOptions<TodoContext>>(services =>
                    services.AddDbContext<TodoContext>(options => options.UseSqlServer(string.Format(connectString, DatabaseName))));

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using var scope = Services.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
                context.Database.EnsureDeleted();
            }
        }
    }
}
