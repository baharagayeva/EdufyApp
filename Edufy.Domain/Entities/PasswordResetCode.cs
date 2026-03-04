namespace Edufy.Domain.Entities;

public class PasswordResetCode
{
    public long Id { get; set; }

    public string Email { get; set; } = default!;
    public string CodeHash { get; set; } = default!;

    public DateTime ExpiresAt { get; set; }
    public DateTime? UsedAt { get; set; }

    public int AttemptCount { get; set; } // brute force limit

    public bool IsActive => UsedAt == null && ExpiresAt > DateTime.UtcNow.AddHours(4) && AttemptCount < 5;
}