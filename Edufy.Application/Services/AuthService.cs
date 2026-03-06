using System.Security.Cryptography;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.AuthDTOs;
using Edufy.Domain.Entities;
using Edufy.Domain.Enums;
using Edufy.Domain.Services;
using Edufy.SqlServer.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Edufy.Application.Services;

public sealed class AuthService(
    EdufyDbContext db,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ITokenService tokens,
    ICurrentUser currentUser,
    IEmailSender emailSender,
    ILogger<AuthService> logger)
    : IAuthService
{
    private static readonly string[] AllowedRoles = ["Teacher", "Student"];

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
            return Result<AuthResponse>.BadRequest("Email is required.");

        if (string.IsNullOrWhiteSpace(req.Password))
            return Result<AuthResponse>.BadRequest("Password is required.");

        var exists = await userManager.Users.AnyAsync(x => x.Email == email, ct);
        if (exists)
            return Result<AuthResponse>.Conflict("This email is already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = req.FullName
        };

        var create = await userManager.CreateAsync(user, req.Password);
        if (!create.Succeeded)
            return Result<AuthResponse>.BadRequest(string.Join("; ", create.Errors.Select(e => e.Description)));

        // No role yet → issue token without role claims
        return await IssueTokensAsync(user, roles: Array.Empty<string>(), ct);
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
        return await IssueTokensAsync(user, roles, ct);
    }

    public async Task<Result<AuthResponse>> SetRoleAsync(UserRole role, CancellationToken ct)
    {
        if (currentUser.UserId == Guid.Empty)
            return Result<AuthResponse>.Unauthorized("Missing or invalid access token.");

        var roleName = role.ToString();
        if (!AllowedRoles.Contains(roleName))
            return Result<AuthResponse>.BadRequest("Invalid role selection.");

        var user = await userManager.FindByIdAsync(currentUser.UserId.ToString());
        if (user is null)
            return Result<AuthResponse>.NotFound("User not found.");

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
            return Result<AuthResponse>.Conflict("Role is already selected.");

        // ensure role exists
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var createRole = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            if (!createRole.Succeeded)
                return Result<AuthResponse>.ServerError("Failed to create role.");
        }

        var add = await userManager.AddToRoleAsync(user, roleName);
        if (!add.Succeeded)
            return Result<AuthResponse>.ServerError(string.Join("; ", add.Errors.Select(e => e.Description)));

        var roles = await userManager.GetRolesAsync(user);
        return await IssueTokensAsync(user, roles, ct);
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
            return Result<AuthResponse>.Unauthorized("Refresh token expired or revoked.");

        // rotation: revoke old, issue new
        rt.RevokedAt = DateTime.UtcNow.AddHours(4);

        var newRefresh = tokens.GenerateRefreshToken();
        var newHash = tokens.Sha256Base64(newRefresh);

        rt.ReplacedByTokenHash = newHash;

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = rt.UserId,
            TokenHash = newHash,
            ExpiresAt = tokens.RefreshTokenExpiresAtUtc()
        });

        await db.SaveChangesAsync(ct);

        var roles = await userManager.GetRolesAsync(rt.User);

        var access = tokens.CreateAccessToken(rt.User, roles);

        var resp = new AuthResponse(access, newRefresh, tokens.AccessTokenExpiresInSeconds(), roles.ToArray(),
            roles.Count == 0);

        return Result<AuthResponse>.Ok(resp);
    }

    public async Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken ct)
    {
        if (currentUser.UserId == Guid.Empty)
            return Result<bool>.Unauthorized("Missing or invalid access token.");

        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<bool>.BadRequest("Refresh token is required.");

        var tokenHash = tokens.Sha256Base64(refreshToken);

        var rt = await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId && x.TokenHash == tokenHash, ct);

        // idempotent
        if (rt is null)
            return Result<bool>.Ok(true, "Logged out.");

        rt.RevokedAt = DateTime.UtcNow.AddHours(4);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Ok(true, "Logged out.");
    }

    public async Task<Result<MeResponse>> MeAsync(CancellationToken ct)
    {
        if (currentUser.UserId == Guid.Empty)
            return Result<MeResponse>.Unauthorized("Missing or invalid access token.");

        var user = await userManager.FindByIdAsync(currentUser.UserId.ToString());
        if (user is null)
            return Result<MeResponse>.NotFound("User not found.");

        var roles = await userManager.GetRolesAsync(user);

        var resp = new MeResponse(
            UserId: user.Id,
            Username: user.UserName ?? string.Empty,
            Email: user.Email ?? "",
            Roles: roles.ToArray(),
            RequiresRoleSelection: roles.Count == 0
        );

        return Result<MeResponse>.Ok(resp);
    }

    public async Task<Result<MessageResponse>> ForgotPasswordAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        try
        {
            var email = (req.Email ?? "").Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
                return Result<MessageResponse>.BadRequest("Email is required.");

            // ✅ anti-enumeration: hər zaman eyni mesaj
            const string genericMsg = "If an account exists for this email, a reset code has been sent.";

            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user is null)
                return Result<MessageResponse>.Ok(new MessageResponse(genericMsg));

            //əvvəlki aktiv kodları deaktiv elə (1 email = 1 aktiv kod)
            var actives = await db.PasswordResetCodes
                .Where(x => x.Email == email && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow.AddHours(4))
                .ToListAsync(ct);
            
            foreach (var c in actives)
                c.UsedAt = DateTime.UtcNow.AddHours(4);

            var code = Generate6DigitCode();
            var codeHash = tokens.Sha256Base64(code);

            db.PasswordResetCodes.Add(new PasswordResetCode
            {
                Email = email,
                CodeHash = codeHash,
                ExpiresAt = DateTime.UtcNow.AddHours(4).AddMinutes(10),
                AttemptCount = 0
            });
            
            await db.SaveChangesAsync(ct);

            await emailSender.SendAsync(
                toEmail: email,
                subject: "Your password reset code",
                htmlBody: $@"
            <p>Your password reset code is:</p>
            <h2 style=""letter-spacing:2px"">{code}</h2>
            <p>This code expires in <b>10 minutes</b>.</p>
            <p>If you did not request this, ignore this email.</p>
        ",
                ct: ct
            );

            return Result<MessageResponse>.Ok(new MessageResponse(genericMsg));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SMTP send failed");
            return Result<MessageResponse>.ServerError("Failed to send reset code. Please try again later.");
        }
    }

    public async Task<Result<MessageResponse>> ResetPasswordWithCodeAsync(ResetPasswordWithCodeRequest req,
        CancellationToken ct)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
            return Result<MessageResponse>.BadRequest("Email is required.");

        var code = (req.Code ?? "").Trim();
        if (string.IsNullOrWhiteSpace(code))
            return Result<MessageResponse>.BadRequest("Code is required.");

        if (string.IsNullOrWhiteSpace(req.NewPassword))
            return Result<MessageResponse>.BadRequest("New password is required.");

        // generic error (anti-enumeration)
        const string invalidMsg = "Invalid or expired code.";

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
        if (user is null)
            return Result<MessageResponse>.BadRequest(invalidMsg);

        // ən son aktiv kod
        var reset = await db.PasswordResetCodes
            .Where(x => x.Email == email && x.UsedAt == null)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(ct);

        if (reset is null || !reset.IsActive)
            return Result<MessageResponse>.BadRequest(invalidMsg);

        // attempt artır (brute force limit)
        reset.AttemptCount++;

        var inputHash = tokens.Sha256Base64(code);
        if (!string.Equals(reset.CodeHash, inputHash, StringComparison.Ordinal))
        {
            await db.SaveChangesAsync(ct);
            return Result<MessageResponse>.BadRequest(invalidMsg);
        }

        // doğru kod → istifadə olundu
        reset.UsedAt = DateTime.UtcNow.AddHours(4);
        await db.SaveChangesAsync(ct);

        // ✅ Identity: token yoxdur deyə Remove+Add edirik
        var hasPassword = await userManager.HasPasswordAsync(user);

        if (hasPassword)
        {
            var remove = await userManager.RemovePasswordAsync(user);
            if (!remove.Succeeded)
                return Result<MessageResponse>.ServerError("Failed to reset password.");
        }

        var add = await userManager.AddPasswordAsync(user, req.NewPassword);
        if (!add.Succeeded)
            return Result<MessageResponse>.BadRequest(string.Join("; ", add.Errors.Select(e => e.Description)));

        // (optional) bütün refresh tokenləri revoke et (security)
        var activeRts = await db.RefreshTokens
            .Where(x => x.UserId == user.Id && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow.AddHours(4))
            .ToListAsync(ct);

        foreach (var rt in activeRts)
            rt.RevokedAt = DateTime.UtcNow.AddHours(4);

        await db.SaveChangesAsync(ct);

        return Result<MessageResponse>.Ok(new MessageResponse("Password has been reset successfully."));
    }

    private async Task<Result<AuthResponse>> IssueTokensAsync(User user, IList<string> roles, CancellationToken ct)
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

        var access = tokens.CreateAccessToken(user, roles);

        if (string.IsNullOrWhiteSpace(access))
            return Result<AuthResponse>.ServerError("Failed to create access token.");

        var resp = new AuthResponse(access, refresh, tokens.AccessTokenExpiresInSeconds(), roles.ToArray(),
            roles.Count == 0
        );

        return Result<AuthResponse>.Ok(resp);
    }

    private static string Generate6DigitCode()
    {
        var n = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return n.ToString("D6");
    }
}