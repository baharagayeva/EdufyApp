namespace Edufy.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? CoverImageUrl { get; set; }

    public int DurationMonths { get; set; }          // "4 ay"
    public int GroupSizeMin { get; set; }            // 15
    public int GroupSizeMax { get; set; }            // 20

    public bool IsPopular { get; set; }
    public bool IsActive { get; set; } = true;

    public int InstructorProfileId { get; set; }
    public InstructorProfile InstructorProfile { get; set; } = default!;

    public ICollection<CourseModule> Modules { get; set; } = new List<CourseModule>();
}