using Edufy.Domain.Enums;

namespace Edufy.Domain.Entities;

public class Application
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int ProgramId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; }
    public DateTime AppliedAt { get; set; }

    public User User { get; set; } = null!;
    public Program Program { get; set; } = null!;
}