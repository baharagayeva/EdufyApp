namespace Edufy.Domain.DTOs.ProgramDTOs;

public record ModuleDto(
    int Id,
    int Order,
    string Name,
    bool IsOpen,
    List<LessonDto> Lessons
);