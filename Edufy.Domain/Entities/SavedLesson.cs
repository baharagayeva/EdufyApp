namespace Edufy.Domain.Entities;

public class SavedLesson
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int LessonId { get; set; }
    public DateTime SavedAt { get; set; }

    public User User { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}
