namespace Edufy.Domain.Abstractions;

public interface ICurrentUser
{
    int UserId { get; }
    string? FullName { get; }
    string? Role { get; }
}