using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTaskManagement.DTOs.Projects;
using ProjectTaskManagement.Services;

namespace ProjectTaskManagement.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(CreateProjectDto request)
    {
        var result = await _projectService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, version = HttpContext.GetRequestedApiVersion()?.ToString() }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectResponseDto>>> GetAll()
    {
        var result = await _projectService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var result = await _projectService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProjectResponseDto>> Update(int id, UpdateProjectDto request)
    {
        var result = await _projectService.UpdateAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _projectService.DeleteAsync(id);
        return NoContent();
    }
}
