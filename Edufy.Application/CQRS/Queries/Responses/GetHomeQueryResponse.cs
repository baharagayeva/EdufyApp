using Edufy.Domain.DTOs.HomeDTOs;

namespace Edufy.Application.CQRS.Queries.Responses;

public record GetHomeQueryResponse(
    string GreetingName,
    IReadOnlyList<CourseCardDto> PopularCourses,
    IReadOnlyList<InstructorCardDto> Instructors
);