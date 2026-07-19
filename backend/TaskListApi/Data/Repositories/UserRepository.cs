using Microsoft.EntityFrameworkCore;
using TaskListApi.Models;

namespace TaskListApi.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUsernameAsync(string username) =>
        _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public Task<bool> ExistsAsync(string username, string email) =>
        _context.Users.AnyAsync(u => u.Username == username || u.Email == email);

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}