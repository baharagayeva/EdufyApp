using System.Security.Claims;
using Edufy.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Edufy.Application.Abstractions;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            var sub = User?.FindFirstValue("sub");
            if (Guid.TryParse(sub, out var idFromSub))
                return idFromSub;

            var nameId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(nameId, out var idFromName))
                return idFromName;

            return Guid.Empty;
        }
    }

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email) ??
        User?.FindFirstValue("email");

    public string? Role => User?.FindFirstValue(ClaimTypes.Role);

    public string? FullName => User?.FindFirstValue(ClaimTypes.Name);
}