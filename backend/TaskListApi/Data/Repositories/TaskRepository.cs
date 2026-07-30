using Microsoft.EntityFrameworkCore;
using TaskListApi.Models;

namespace TaskListApi.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<TaskItem>> GetAllByUserAsync(Guid userId) =>
        _context.Tasks
        .FromSqlInterpolated($"SELECT * FROM get_tasks_by_user({userId})")
        .ToListAsync();

    public Task<TaskItem?> GetByIdAsync(Guid id, Guid userId) =>
        _context.Tasks
        .FromSqlInterpolated($"SELECT * FROM get_task_by_id({id}, {userId})")
        .FirstOrDefaultAsync();

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task UpdateAsync(TaskItem task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}