using ProjectTaskManagement.DTOs.Tasks;

namespace ProjectTaskManagement.Services;

public interface ITaskService
{
    Task<TaskResponseDto> CreateAsync(int projectId, CreateTaskDto dto);
    Task<IReadOnlyList<TaskResponseDto>> GetByProjectAsync(int projectId);
    Task<TaskResponseDto> UpdateStatusAsync(int projectId, int taskId, UpdateTaskStatusDto dto);
    Task DeleteAsync(int projectId, int taskId);
}
