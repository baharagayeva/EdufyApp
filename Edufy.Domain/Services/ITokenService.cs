using System;
using System.Collections.Generic;
using Edufy.Domain.Entities;

namespace Edufy.Domain.Services;

public interface ITokenService
{
    string CreateAccessToken(User user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    string Sha256Base64(string input);
    int AccessTokenExpiresInSeconds();
    DateTime RefreshTokenExpiresAtUtc();
}