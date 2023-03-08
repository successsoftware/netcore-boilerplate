using CleanArchitecture.Net7.WebApi.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Net7.WebApi.Data
{
    public class AppIdentityDbContext : GenericContext<AppIdentityDbContext>, IAppDbContext
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        public override void DatabaseConfig(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
    }
}