using ProjectTaskManagement.DTOs.Projects;

namespace ProjectTaskManagement.Services;

public interface IProjectService
{
    Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);
    Task<IReadOnlyList<ProjectResponseDto>> GetAllAsync();
    Task<ProjectResponseDto> GetByIdAsync(int projectId);
    Task<ProjectResponseDto> UpdateAsync(int projectId, UpdateProjectDto dto);
    Task DeleteAsync(int projectId);
}
