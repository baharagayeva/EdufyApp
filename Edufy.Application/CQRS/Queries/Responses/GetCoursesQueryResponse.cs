namespace Edufy.Application.CQRS.Queries.Responses;

public record GetCoursesQueryResponse(
    int Id,
    string Title,
    string DurationText,
    bool IsActive,
    string? CoverImageUrl
);