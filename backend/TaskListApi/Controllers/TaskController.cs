using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskListApi.DTOs;
using TaskListApi.Models;
using TaskListApi.Services;

namespace TaskListApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.Parse(sub!);
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetAll()
    {
        var tasks = await _taskService.GetAllForUserAsync(GetCurrentUserId());
        return Ok(tasks.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetById(Guid id)
    {
        var task = await _taskService.GetByIdForUserAsync(id, GetCurrentUserId());
        if (task is null) return NotFound();
        return Ok(ToResponse(task));
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var task = await _taskService.CreateAsync(GetCurrentUserId(), request);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, ToResponse(task));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> Update(Guid id, UpdateTaskRequest request)
    {
        var task = await _taskService.UpdateAsync(id, GetCurrentUserId(), request);
        if (task is null) return NotFound();
        return Ok(ToResponse(task));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _taskService.DeleteAsync(id, GetCurrentUserId());
        if (!deleted) return NotFound();
        return NoContent();
    }

    private static TaskResponse ToResponse(TaskItem task) => new(
        task.Id, task.Title, task.Description, task.DueDate,
        task.Priority, task.Category, task.Status, task.CreatedAt, task.UpdatedAt
    );
}
