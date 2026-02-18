using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Edufy.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Edufy.Application.Abstractions;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public bool IsAuthenticated =>
        accessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public Guid? UserId
    {
        get
        {
            var user = accessor.HttpContext?.User;
            if (user is null) return null;

            var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (Guid.TryParse(sub, out var idFromSub))
                return idFromSub;

            var nameId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(nameId, out var idFromNameId))
                return idFromNameId;

            return null;
        }
    }
    
    public string? FullName => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
    public string? Role => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
}