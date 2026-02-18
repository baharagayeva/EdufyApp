using Edufy.Domain.Entities;

namespace Edufy.Domain.Services;

public interface ITokenService
{
    string CreateAccessToken(User user, IReadOnlyList<string> roles);
    string GenerateRefreshToken();
    string Sha256Base64(string input);
    int AccessTokenExpiresInSeconds();
    DateTime RefreshTokenExpiresAtUtc();
}