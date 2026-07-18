namespace TaskListApi.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }

    public required TaskPriority Priority { get; set; }
    public string? Category { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}