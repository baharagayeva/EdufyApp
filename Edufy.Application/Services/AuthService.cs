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
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ITokenService tokens)
    : IAuthService
{
    private static readonly string[] AllowedRoles = ["Teacher", "Student"];

    public async Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        var exists = await userManager.Users.AnyAsync(x => x.Email == email, ct);
        if (exists) throw new Exception("Bu email artıq mövcuddur");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = email
        };

        var create = await userManager.CreateAsync(user, req.Password);
        if (!create.Succeeded)
            throw new Exception(string.Join("; ", create.Errors.Select(e => e.Description)));

        // Rol YOX → token rol claim-siz veriləcək
        return await IssueTokensAsync(user, roles: [], ct);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email, ct)
                   ?? throw new Exception("Email və ya şifrə yanlışdır");

        var ok = await userManager.CheckPasswordAsync(user, req.Password);
        if (!ok) throw new Exception("Email və ya şifrə yanlışdır");

        var roles = await userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user, roles, ct);
    }

    public async Task<AuthResponse> SetRoleAsync(Guid userId, UserRole role, CancellationToken ct)
    {
        if (!AllowedRoles.Contains(role.ToString()))
            throw new Exception("Yanlış rol seçildi");

        var user = await userManager.FindByIdAsync(userId.ToString())
                   ?? throw new Exception("User tapılmadı");

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
            throw new Exception("Rol artıq seçilib"); // istəsən change-role flow edə bilərsən

        // Role yoxdursa yaradaq
        var roleExists = await roleManager.RoleExistsAsync(role.ToString());
        if (!roleExists)
            await roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));

        var add = await userManager.AddToRoleAsync(user, role.ToString());
        if (!add.Succeeded)
            throw new Exception(string.Join("; ", add.Errors.Select(e => e.Description)));

        // Rol seçildi → yeni tokenlər (role claim ilə)
        var roles = await userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user, roles, ct);
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var tokenHash = tokens.Sha256Base64(refreshToken);

        var rt = await db.RefreshTokens
                     .Include(x => x.User)
                     .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct)
                 ?? throw new Exception("Refresh token etibarsızdır");

        if (!rt.IsActive) throw new Exception("Refresh token müddəti bitib və ya ləğv edilib");

        var user = rt.User;

        // rotation: köhnəni revoke elə, yenini yarat
        rt.RevokedAt = DateTime.UtcNow;

        var newRefresh = tokens.GenerateRefreshToken();
        var newHash = tokens.Sha256Base64(newRefresh);

        rt.ReplacedByTokenHash = newHash;

        var newRt = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = newHash,
            ExpiresAt = tokens.RefreshTokenExpiresAtUtc()
        };

        db.RefreshTokens.Add(newRt);
        await db.SaveChangesAsync(ct);

        var roles = await userManager.GetRolesAsync(user);
        var access = tokens.CreateAccessToken(user, (IReadOnlyList<string>)roles);

        return new AuthResponse
        {
            AccessToken = access,
            RefreshToken = newRefresh,
            ExpiresInSeconds = tokens.AccessTokenExpiresInSeconds(),
            Roles = roles.ToArray(),
            RequiresRoleSelection = roles.Count == 0
        };
    }

    public async Task LogoutAsync(Guid userId, string refreshToken, CancellationToken ct)
    {
        var tokenHash = tokens.Sha256Base64(refreshToken);

        var rt = await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TokenHash == tokenHash, ct);

        if (rt == null) return;

        rt.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    public async Task<(Guid UserId, string Email, string[] Roles, bool RequiresRoleSelection)> MeAsync(Guid userId,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(userId.ToString())
                   ?? throw new Exception("User tapılmadı");

        var roles = await userManager.GetRolesAsync(user);
        return (user.Id, user.Email ?? "", roles.ToArray(), roles.Count == 0);
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, IList<string> roles, CancellationToken ct)
    {
        // refresh token DB-yə hash olaraq yazılır
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

        return new AuthResponse
        {
            AccessToken = access,
            RefreshToken = refresh,
            ExpiresInSeconds = tokens.AccessTokenExpiresInSeconds(),
            Roles = roles.ToArray(),
            RequiresRoleSelection = roles.Count == 0
        };
    }
}