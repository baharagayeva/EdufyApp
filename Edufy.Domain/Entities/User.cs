using Microsoft.AspNetCore.Identity;

namespace Edufy.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}