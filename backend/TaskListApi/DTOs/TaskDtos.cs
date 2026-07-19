using TaskListApi.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskListApi.DTOs;

public record CreateTaskRequest(
    [Required, MaxLength(200)] string Title,
    [MaxLength(2000)] string? Description,
    DateTime? DueDate,
    TaskPriority Priority,
    [MaxLength(100)] string? Category
);

public record UpdateTaskRequest(
    [Required, MaxLength(200)] string Title,
    [MaxLength(2000)] string? Description,
    DateTime? DueDate,
    TaskPriority Priority,
    [MaxLength(100)] string? Category,
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