using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TaskListApi.Data.Repositories;
using System.Security.Claims;
using TaskListApi.DTOs;
using TaskListApi.Models;

namespace TaskListApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    private Guid GetCurrentUserId()
    {
        var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.Parse(sub!);
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetAll()
    {
        var tasks = await _taskRepository.GetAllByUserAsync(GetCurrentUserId());
        return Ok(tasks.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetById(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id, GetCurrentUserId());
        if (task is null) return NotFound();
        return Ok(ToResponse(task));
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            UserId = GetCurrentUserId(),
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Category = request.Category
        };

        await _taskRepository.AddAsync(task);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, ToResponse(task));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> Update(Guid id, UpdateTaskRequest request)
    {
        var task = await _taskRepository.GetByIdAsync(id, GetCurrentUserId());
        if (task is null) return NotFound();

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.Priority = request.Priority;
        task.Category = request.Category;
        task.Status = request.Status;

        await _taskRepository.UpdateAsync(task);

        return Ok(ToResponse(task));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id, GetCurrentUserId());
        if (task is null) return NotFound();

        await _taskRepository.DeleteAsync(task);

        return NoContent();
    }

    private static TaskResponse ToResponse(TaskItem task) => new(
        task.Id, task.Title, task.Description, task.DueDate,
        task.Priority, task.Category, task.Status, task.CreatedAt, task.UpdatedAt
    );
}