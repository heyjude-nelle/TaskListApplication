namespace TaskListApi.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Username { get; set; }
    public required string Email { get; set; }

    public string? PasswordHash { get; set; }

    public string AuthProvider { get; set; } = "Local";
    public string? ProviderUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}