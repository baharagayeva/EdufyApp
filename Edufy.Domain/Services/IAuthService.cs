using Edufy.Domain.DTOs;
using Edufy.Domain.Enums;

namespace Edufy.Domain.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct);
    Task<AuthResponse> RefreshAsync(string refreshToken, CancellationToken ct);
    Task LogoutAsync(Guid userId, string refreshToken, CancellationToken ct);
    Task<AuthResponse> SetRoleAsync(Guid userId, UserRole role, CancellationToken ct);
    Task<(Guid UserId, string Email, string[] Roles, bool RequiresRoleSelection)> MeAsync(Guid userId, CancellationToken ct);
}