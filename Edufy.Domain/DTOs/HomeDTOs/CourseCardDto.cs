namespace Edufy.Domain.DTOs.HomeDTOs;

public sealed record CourseCardDto(
    int Id,
    string Title,
    string DurationText,
    bool IsActive,
    string? CoverImageUrl
);