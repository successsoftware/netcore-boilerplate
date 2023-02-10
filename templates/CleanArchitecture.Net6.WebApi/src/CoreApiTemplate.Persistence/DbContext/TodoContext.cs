using CoreApiTemplate.Application.Interfaces;
using CoreApiTemplate.Domain.Entities;
using CoreApiTemplate.Persistence.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SSS.CommonLib.Interfaces;
using SSS.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoreApiTemplate.Persistence.DbContext
{
    public class TodoContext : AuditContext, IAppDbContext
    {
        public TodoContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor,
            IDateTimeService dateTimeService)
            : base(options, httpContextAccessor, dateTimeService)
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoItemConfiguration).Assembly);
        }

        public async Task CommitAsync(Func<Task> action)
        {
            var strategy = Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = Database.BeginTransaction();

                try
                {
                    await base.SaveChangesAsync();

                    await action();
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message, ex);

                    transaction.Rollback();

                    throw;
                }
                finally
                {
                    transaction.Commit();
                }
            });
        }
    }
}