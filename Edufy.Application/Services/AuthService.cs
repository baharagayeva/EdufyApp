using Edufy.Domain.Abstractions;
using Edufy.Domain.Common;
using Edufy.Domain.DTOs;
using Edufy.Domain.Entities;
using Edufy.Domain.Enums;
using Edufy.Domain.Services;
using Edufy.SqlServer.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.Services;

public class AuthService(
    EdufyDbContext db,
    ICurrentUser currentUser,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ITokenService tokens)
    : IAuthService
{
    private static readonly string[] AllowedRoles = ["Teacher", "Student"];

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
            return Result<AuthResponse>.BadRequest("Email is required.");

        var exists = await userManager.Users.AnyAsync(x => x.Email == email, ct);
        if (exists)
            return Result<AuthResponse>.Conflict("This email is already in use.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = req.FullName
        };

        var create = await userManager.CreateAsync(user, req.Password);
        if (!create.Succeeded)
            return Result<AuthResponse>.BadRequest(string.Join("; ", create.Errors.Select(e => e.Description)));

        // No role yet -> tokens without role claim (RequiresRoleSelection = true)
        return await IssueTokensAsync(user, roles: Array.Empty<string>(), created: true, ct);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest req, CancellationToken ct)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
            return Result<AuthResponse>.BadRequest("Email is required.");

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
        if (user is null)
            return Result<AuthResponse>.Unauthorized("Invalid email or password.");

        var ok = await userManager.CheckPasswordAsync(user, req.Password);
        if (!ok)
            return Result<AuthResponse>.Unauthorized("Invalid email or password.");

        var roles = await userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user, roles, created: false, ct);
    }

    public async Task<Result<AuthResponse>> SetRoleAsync(Guid userId, UserRole role, CancellationToken ct)
    {
        var roleName = role.ToString();

        if (!AllowedRoles.Contains(roleName))
            return Result<AuthResponse>.BadRequest("Invalid role selected.");

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<AuthResponse>.NotFound("User not found.");

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
            return Result<AuthResponse>.Conflict("Role is already selected.");

        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var created = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            if (!created.Succeeded)
                return Result<AuthResponse>.ServerError(string.Join("; ", created.Errors.Select(e => e.Description)));
        }

        var add = await userManager.AddToRoleAsync(user, roleName);
        if (!add.Succeeded)
            return Result<AuthResponse>.BadRequest(string.Join("; ", add.Errors.Select(e => e.Description)));

        var roles = await userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user, roles, created: false, ct);
    }

    public async Task<Result<AuthResponse>> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<AuthResponse>.BadRequest("Refresh token is required.");

        var tokenHash = tokens.Sha256Base64(refreshToken);

        var rt = await db.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

        if (rt is null)
            return Result<AuthResponse>.Unauthorized("Invalid refresh token.");

        if (!rt.IsActive)
            return Result<AuthResponse>.Unauthorized("Refresh token has expired or was revoked.");

        var user = rt.User;

        // rotation: revoke old, create new
        rt.RevokedAt = DateTime.UtcNow;

        var newRefresh = tokens.GenerateRefreshToken();
        var newHash = tokens.Sha256Base64(newRefresh);

        rt.ReplacedByTokenHash = newHash;

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = newHash,
            ExpiresAt = tokens.RefreshTokenExpiresAtUtc()
        });

        await db.SaveChangesAsync(ct);

        var roles = await userManager.GetRolesAsync(user);
        var access = tokens.CreateAccessToken(user, (IReadOnlyList<string>)roles);

        if (string.IsNullOrWhiteSpace(access))
            return Result<AuthResponse>.ServerError("Failed to generate access token.");

        return Result<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = access,
            RefreshToken = newRefresh,
            ExpiresInSeconds = tokens.AccessTokenExpiresInSeconds(),
            Roles = roles.ToArray(),
            RequiresRoleSelection = roles.Count == 0
        });
    }

    public async Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null)
            return Result<bool>.Unauthorized("Missing or invalid access token.");

        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<bool>.BadRequest("Refresh token is required.");

        var userId = currentUser.UserId.Value;
        var tokenHash = tokens.Sha256Base64(refreshToken);

        var rt = await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TokenHash == tokenHash, ct);

        // idempotent: tapılmasa belə OK
        if (rt is null)
            return Result<bool>.Ok(true, "Logged out.");

        rt.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result<bool>.Ok(true, "Logged out.");
    }

    public async Task<Result<(Guid UserId, string Email, string[] Roles, bool RequiresRoleSelection)>> MeAsync(Guid userId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<(Guid UserId, string Email, string[] Roles, bool RequiresRoleSelection)>.NotFound("User not found.");

        var roles = await userManager.GetRolesAsync(user);

        return Result<(Guid UserId, string Email, string[] Roles, bool RequiresRoleSelection)>.Ok(new ValueTuple<Guid, string, string[], bool>(
            user.Id,
            user.Email ?? "",
            roles.ToArray(),
            roles.Count == 0
        ));
    }

    private async Task<Result<AuthResponse>> IssueTokensAsync(User user, IList<string> roles, bool created, CancellationToken ct)
    {
        var refresh = tokens.GenerateRefreshToken();
        var refreshHash = tokens.Sha256Base64(refresh);

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshHash,
            ExpiresAt = tokens.RefreshTokenExpiresAtUtc()
        });

        await db.SaveChangesAsync(ct);

        var access = tokens.CreateAccessToken(user, (IReadOnlyList<string>)roles);
        if (string.IsNullOrWhiteSpace(access))
            return Result<AuthResponse>.ServerError("Failed to generate access token.");

        var payload = new AuthResponse
        {
            AccessToken = access,
            RefreshToken = refresh,
            ExpiresInSeconds = tokens.AccessTokenExpiresInSeconds(),
            Roles = roles.ToArray(),
            RequiresRoleSelection = roles.Count == 0
        };

        return created
            ? Result<AuthResponse>.Created(payload, "User registered successfully.")
            : Result<AuthResponse>.Ok(payload);
    }
}