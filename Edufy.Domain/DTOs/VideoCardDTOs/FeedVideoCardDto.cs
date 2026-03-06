namespace Edufy.Domain.DTOs.VideoCardDTOs;

public sealed record FeedVideoCardDto(
    int LessonId,
    string LessonName,
    string? ThumbnailUrl,
    int DurationMinutes,
    string VideoUrl,
    int InstructorId,
    string InstructorName,
    int ProgramId,
    string ProgramName,
    bool IsLiked
);