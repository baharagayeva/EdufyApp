namespace Edufy.Domain.Entities;

public class Lesson
{
    public int Id { get; set; }
    public int CourseModuleId { get; set; }
    public CourseModule CourseModule { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string? Content { get; set; } // detail page üçün
    public int Order { get; set; }
}