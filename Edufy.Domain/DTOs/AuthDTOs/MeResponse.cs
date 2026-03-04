namespace Edufy.Domain.DTOs.AuthDTOs;

public sealed record MeResponse(
    Guid UserId,
    string Username,
    string Email,
    string[] Roles,
    bool RequiresRoleSelection
);