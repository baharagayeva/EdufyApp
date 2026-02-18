namespace Edufy.Domain.Abstractions;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Guid? UserId { get; }
    string? FullName { get; }
    string? Role { get; }
}