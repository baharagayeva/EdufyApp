using System.Security.Claims;
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
    public Task<AuthResponse> Register(RegisterRequest req, CancellationToken ct)
        => auth.RegisterAsync(req, ct);

    [HttpPost("login")]
    public Task<AuthResponse> Login(LoginRequest req, CancellationToken ct)
        => auth.LoginAsync(req, ct);

    [HttpPost("refresh")]
    public Task<AuthResponse> Refresh(RefreshRequest req, CancellationToken ct)
        => auth.RefreshAsync(req.RefreshToken, ct);

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest req, CancellationToken ct)
    {
        var userId = GetUserId();
        await auth.LogoutAsync(userId, req.RefreshToken, ct);
        return Ok();
    }

    [Authorize]
    [HttpPost("set-role")]
    public Task<AuthResponse> SetRole(SetRoleRequest req, CancellationToken ct)
        => auth.SetRoleAsync(GetUserId(), req.Role, ct);

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var me = await auth.MeAsync(GetUserId(), ct);
        return Ok(new
        {
            me.UserId,
            me.Email,
            me.Roles,
            me.RequiresRoleSelection
        });
    }

    private Guid GetUserId()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(id!);
    }
}