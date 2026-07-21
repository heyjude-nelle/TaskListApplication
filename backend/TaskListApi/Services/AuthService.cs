using Microsoft.AspNetCore.Identity;
using TaskListApi.Data.Repositories;
using TaskListApi.DTOs;
using TaskListApi.Models;

namespace TaskListApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsAsync(request.Username, request.Email))
        {
            return null;
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse(token, user.Username, user.Email);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse(token, user.Username, user.Email);
    }
}