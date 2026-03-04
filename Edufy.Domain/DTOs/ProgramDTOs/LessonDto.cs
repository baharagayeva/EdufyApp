namespace Edufy.Domain.DTOs.ProgramDTOs;

public record LessonDto(
    int Id,
    int Order,
    string Name,
    int DurationMinutes
);