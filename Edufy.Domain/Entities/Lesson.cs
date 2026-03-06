namespace Edufy.Domain.Entities;

public class Lesson
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsDemo { get; set; }             
    public string? ThumbnailUrl { get; set; }

    public Module Module { get; set; } = null!;
    public ICollection<SavedLesson> SavedLessons { get; set; } = [];
}