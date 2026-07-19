using TaskListApi.Models;

namespace TaskListApi.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}