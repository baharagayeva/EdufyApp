using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.AuthDTOs;
using Edufy.Domain.Enums;

namespace Edufy.Domain.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest req, CancellationToken ct);
    Task<Result<AuthResponse>> RefreshAsync(string refreshToken, CancellationToken ct);

    Task<Result<AuthResponse>> SetRoleAsync(UserRole role, CancellationToken ct);
    Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken ct);
    Task<Result<MeResponse>> MeAsync(CancellationToken ct);
}