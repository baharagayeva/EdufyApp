using System.Security.Claims;
using Edufy.Domain.Common;
using Edufy.Domain.DTOs;
using Edufy.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edufy.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
        => (await auth.RegisterAsync(req, ct)).ToActionResult();

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
        => (await auth.LoginAsync(req, ct)).ToActionResult();
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken, CancellationToken ct)
        => (await auth.RefreshAsync(refreshToken, ct)).ToActionResult();

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest req, CancellationToken ct)
        => (await auth.LogoutAsync(req.RefreshToken, ct)).ToActionResult();

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var sub = User.FindFirstValue("sub");
        if (!Guid.TryParse(sub, out var userId))
            return Unauthorized();

        return (await auth.MeAsync(userId, ct)).ToActionResult();
    }
}