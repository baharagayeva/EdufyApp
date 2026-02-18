namespace Edufy.Domain.Entities;

public class InstructorProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public string Title { get; set; } = default!; // "iOS instructor"
    public string Bio { get; set; } = default!;
    public string? PhotoUrl { get; set; }
    public string? LinkedInUrl { get; set; }
}