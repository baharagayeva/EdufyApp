namespace Edufy.Domain.DTOs.InstructorDTOs;

public sealed record DemoVideoCardDto(
    int LessonId,
    string LessonName,
    string? ThumbnailUrl,
    int DurationMinutes,
    string VideoUrl,
    string ProgramName,
    int ProgramId,
    bool IsLiked
);