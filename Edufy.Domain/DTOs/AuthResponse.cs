namespace Edufy.Domain.DTOs;

public class AuthResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public int ExpiresInSeconds { get; set; }

    public bool RequiresRoleSelection { get; set; }
    public string[] Roles { get; set; } = [];
}