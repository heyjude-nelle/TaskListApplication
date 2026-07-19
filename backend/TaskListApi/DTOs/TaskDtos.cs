using TaskListApi.Models;

namespace TaskListApi.DTOs;

public record CreateTaskRequest(
    string Title,
    string? Description,
    DateTime? DueDate,
    TaskPriority Priority,
    string? Category
);

public record UpdateTaskRequest(
    string Title,
    string? Description,
    DateTime? DueDate,
    TaskPriority Priority,
    string? Category,
    TaskItemStatus Status
);

public record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    DateTime? DueDate,
    TaskPriority Priority,
    string? Category,
    TaskItemStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);