using MathTestSystem.Shared.DTOs;
using MathTestSystem.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest("Username and password are required");

            var token = await _authService.AuthenticateAsync(login.Username, login.Password);

            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for user {Username}", login.Username);
            return StatusCode(500, "An unexpected error occurred");
        }
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Me()
    {
        try
        {
            // Get username from JWT
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _authService.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user info");
            return StatusCode(500, "Internal server error");
        }
    }
}