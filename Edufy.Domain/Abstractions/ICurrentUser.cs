namespace Edufy.Domain.Abstractions;

public interface ICurrentUser
{
    Guid UserId { get; }
    string? Email { get; }
    string? Role { get; }
    string? FullName { get; }
}