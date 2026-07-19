using TaskListApi.Models;

namespace TaskListApi.Data.Repositories;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllByUserAsync(Guid userId);
    Task<TaskItem?> GetByIdAsync(Guid id, Guid userId);
    Task<TaskItem> AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}