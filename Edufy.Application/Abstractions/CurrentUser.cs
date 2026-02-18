using System.Security.Claims;
using Edufy.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Edufy.Application.Abstractions;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public int UserId => int.Parse(accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    public string? FullName => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
    public string? Role => accessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
}