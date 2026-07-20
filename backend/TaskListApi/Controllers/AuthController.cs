using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskListApi.Services;
using TaskListApi.DTOs;
using System.Security.Claims;


namespace TaskListApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

   [HttpPost("register")]
public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
{
    var result = await _authService.RegisterAsync(request);

    if (result is null)
    {
        return Conflict("Username or email is already taken.");
    }

    return Ok(result);
}

[HttpPost("login")]
public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
{
    var result = await _authService.LoginAsync(request);

    if (result is null)
    {
        return Unauthorized();
    }

    return Ok(result);
}

    [Authorize]
    [HttpGet("profile")]
    public ActionResult UserProfile()
    {
        var username = User.FindFirstValue("unique_name");
        var email = User.FindFirstValue("email");

        return Ok(new { username, email });
    }

}