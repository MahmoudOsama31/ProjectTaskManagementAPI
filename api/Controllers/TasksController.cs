using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.DTOs.Tasks;
using ProjectTaskManagement.Services;

namespace ProjectTaskManagement.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects/{projectId:int}/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(int projectId, CreateTaskDto request)
    {
        var result = await _taskService.CreateAsync(projectId, request);
        return CreatedAtAction(nameof(GetByProject), new { projectId, version = HttpContext.GetRequestedApiVersion()?.ToString() }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskResponseDto>>> GetByProject(int projectId)
    {
        var result = await _taskService.GetByProjectAsync(projectId);
        return Ok(result);
    }

    [HttpPatch("{taskId:int}/status")]
    public async Task<ActionResult<TaskResponseDto>> UpdateStatus(
        int projectId,
        int taskId,
        UpdateTaskStatusDto request)
    {
        var result = await _taskService.UpdateStatusAsync(projectId, taskId, request);
        return Ok(result);
    }

    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> Delete(int projectId, int taskId)
    {
        await _taskService.DeleteAsync(projectId, taskId);
        return NoContent();
    }
}
