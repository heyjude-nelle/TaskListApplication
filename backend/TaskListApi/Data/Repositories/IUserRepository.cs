using TaskListApi.Models;

namespace TaskListApi.Data.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username, string email);
    Task<User> AddAsync(User user);
}