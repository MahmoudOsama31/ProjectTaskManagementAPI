using Microsoft.EntityFrameworkCore;
using ProjectTaskManagement.Models;

namespace ProjectTaskManagement.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Project> Projects { get; }
    DbSet<TaskItem> Tasks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
