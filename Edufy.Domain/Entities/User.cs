using Microsoft.AspNetCore.Identity;

namespace Edufy.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    
    public ICollection<Application> Applications { get; set; } = [];
    public ICollection<SavedProgram> SavedPrograms { get; set; } = [];
}