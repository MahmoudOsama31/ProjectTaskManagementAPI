using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectTaskManagement.DTOs.Projects;
using ProjectTaskManagement.Exceptions;
using ProjectTaskManagement.Models;
using ProjectTaskManagement.Persistence;

namespace ProjectTaskManagement.Services;

public class ProjectService : ServiceBase, IProjectService
{
    private readonly IApplicationDbContext _context;

    public ProjectService(IApplicationDbContext context, ILogger<ProjectService> logger)
        : base(logger)
    {
        _context = context;
    }

    public Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto) =>
        ExecuteAsync(async () =>
        {
            var project = new Project
            {
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return MapToDto(project);
        });

    public Task<IReadOnlyList<ProjectResponseDto>> GetAllAsync() =>
        ExecuteAsync(async () =>
        {
            var projects = await _context.Projects
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return (IReadOnlyList<ProjectResponseDto>)projects.Select(MapToDto).ToList();
        });

    public Task<ProjectResponseDto> GetByIdAsync(int projectId) =>
        ExecuteAsync(async () =>
        {
            var project = await GetProjectOrThrowAsync(projectId);
            return MapToDto(project);
        });

    public Task<ProjectResponseDto> UpdateAsync(int projectId, UpdateProjectDto dto) =>
        ExecuteAsync(async () =>
        {
            var project = await GetProjectOrThrowAsync(projectId, tracking: true);

            project.Name = dto.Name.Trim();
            project.Description = dto.Description?.Trim();

            await _context.SaveChangesAsync();

            return MapToDto(project);
        });

    public Task DeleteAsync(int projectId) =>
        ExecuteAsync(async () =>
        {
            var project = await GetProjectOrThrowAsync(projectId, tracking: true);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        });

    private async Task<Project> GetProjectOrThrowAsync(int projectId, bool tracking = false)
    {
        var query = tracking ? _context.Projects : _context.Projects.AsNoTracking();

        var project = await query.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null)
        {
            throw new AppException("Project not found.", HttpErrors.NotFound);
        }

        return project;
    }

    private static ProjectResponseDto MapToDto(Project project) =>
        new()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
}
