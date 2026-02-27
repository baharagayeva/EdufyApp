namespace Edufy.Domain.DTOs.AuthDTOs;

public class AuthResponse(string access, string newRefresh, int accessTokenExpiresInSeconds, string[] toArray, bool b)
{
    public string AccessToken { get; set; } = access;
    public string RefreshToken { get; set; } = newRefresh;
    public int ExpiresInSeconds { get; set; } = accessTokenExpiresInSeconds;

    public bool RequiresRoleSelection { get; set; } = b;
    public string[] Roles { get; set; } = toArray;
}