using Edufy.Domain.Enums;

namespace Edufy.Domain.Entities;

public class CourseApplication
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public int StudentUserId { get; set; }
    public User Student { get; set; } = default!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}