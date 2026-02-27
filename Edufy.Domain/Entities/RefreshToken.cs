namespace Edufy.Domain.Entities;

public class RefreshToken
{
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string TokenHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByTokenHash { get; set; }

    public bool IsActive => RevokedAt == null && ExpiresAt > DateTime.UtcNow;
}