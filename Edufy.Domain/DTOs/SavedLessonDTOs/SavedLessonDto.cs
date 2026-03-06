namespace Edufy.Domain.DTOs.SavedLessonDTOs;

public sealed record SavedLessonDto(
    int Id,
    int LessonId,
    string LessonName,
    string? ThumbnailUrl,
    int DurationMinutes,
    string? VideoUrl,
    int ProgramId,
    string ProgramName,
    int InstructorId,
    string InstructorFullName,
    DateTime SavedAt
);