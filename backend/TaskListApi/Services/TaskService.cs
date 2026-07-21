using TaskListApi.Data.Repositories;
using TaskListApi.DTOs;
using TaskListApi.Models;

namespace TaskListApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public Task<List<TaskItem>> GetAllForUserAsync(Guid userId) =>
        _taskRepository.GetAllByUserAsync(userId);

    public Task<TaskItem?> GetByIdForUserAsync(Guid id, Guid userId) =>
        _taskRepository.GetByIdAsync(id, userId);

    public async Task<TaskItem> CreateAsync(Guid userId, CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Category = request.Category
        };

        await _taskRepository.AddAsync(task);

        return task;
    }

    public async Task<TaskItem?> UpdateAsync(Guid id, Guid userId, UpdateTaskRequest request)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);
        if (task is null)
        {
            return null;
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.Priority = request.Priority;
        task.Category = request.Category;
        task.Status = request.Status;

        await _taskRepository.UpdateAsync(task);

        return task;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var task = await _taskRepository.GetByIdAsync(id, userId);
        if (task is null)
        {
            return false;
        }

        await _taskRepository.DeleteAsync(task);

        return true;
    }
}
