using CoreApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApiTemplate.Application.Interfaces
{
    public interface IAppDbContext : IDisposable
    {
        DatabaseFacade Database { get; }
        DbSet<ToDoItem> ToDoItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        Task CommitAsync(Func<Task> action);
    }
}
