using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CoreApiTemplate.Persistence.DbContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<TodoContext>();

            var connectionString = configuration.GetConnectionString("DesignTimeDbConnectionString");

            builder.UseNpgsql(connectionString);

            return new TodoContext(builder.Options, null, null);
        }
    }
}
