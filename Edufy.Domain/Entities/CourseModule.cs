namespace Edufy.Domain.Entities;

public class CourseModule
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public string Title { get; set; } = default!;
    public int Order { get; set; }

    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}