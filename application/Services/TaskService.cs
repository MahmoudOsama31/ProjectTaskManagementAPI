using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectTaskManagement.DTOs.Tasks;
using ProjectTaskManagement.Exceptions;
using ProjectTaskManagement.Models;
using ProjectTaskManagement.Persistence;

namespace ProjectTaskManagement.Services;

public class TaskService : ServiceBase, ITaskService
{
    private readonly IApplicationDbContext _context;

    public TaskService(IApplicationDbContext context, ILogger<TaskService> logger)
        : base(logger)
    {
        _context = context;
    }

    public Task<TaskResponseDto> CreateAsync(int projectId, CreateTaskDto dto) =>
        ExecuteAsync(async () =>
        {
            await EnsureProjectExistsAsync(projectId);

            var task = new TaskItem
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                Status = dto.Status,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                ProjectId = projectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        });

    public Task<IReadOnlyList<TaskResponseDto>> GetByProjectAsync(int projectId) =>
        ExecuteAsync(async () =>
        {
            await EnsureProjectExistsAsync(projectId);

            var tasks = await _context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Priority)
                .ToListAsync();

            return (IReadOnlyList<TaskResponseDto>)tasks.Select(MapToDto).ToList();
        });

    public Task<TaskResponseDto> UpdateStatusAsync(int projectId, int taskId, UpdateTaskStatusDto dto) =>
        ExecuteAsync(async () =>
        {
            await EnsureProjectExistsAsync(projectId);

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (task is null)
            {
                throw new AppException("Task not found.", HttpErrors.NotFound);
            }

            task.Status = dto.Status;
            await _context.SaveChangesAsync();

            return MapToDto(task);
        });

    public Task DeleteAsync(int projectId, int taskId) =>
        ExecuteAsync(async () =>
        {
            await EnsureProjectExistsAsync(projectId);

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (task is null)
            {
                throw new AppException("Task not found.", HttpErrors.NotFound);
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        });

    private async Task EnsureProjectExistsAsync(int projectId)
    {
        var exists = await _context.Projects.AsNoTracking().AnyAsync(p => p.Id == projectId);
        if (!exists)
        {
            throw new AppException("Project not found.", HttpErrors.NotFound);
        }
    }

    private static TaskResponseDto MapToDto(TaskItem task) =>
        new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            Priority = task.Priority,
            ProjectId = task.ProjectId
        };
}
