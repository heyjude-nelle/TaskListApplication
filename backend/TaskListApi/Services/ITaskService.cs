using TaskListApi.Models;
using TaskListApi.DTOs;

namespace TaskListApi.Services;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllForUserAsync(Guid userId);
    Task<TaskItem?> GetByIdForUserAsync(Guid id, Guid userId);
    Task<TaskItem> CreateAsync(Guid userId, CreateTaskRequest request);
    Task<TaskItem?> UpdateAsync(Guid id, Guid userId, UpdateTaskRequest request);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
