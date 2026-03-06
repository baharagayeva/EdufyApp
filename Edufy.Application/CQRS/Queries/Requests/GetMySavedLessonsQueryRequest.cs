using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.SavedLessonDTOs;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public sealed record GetMySavedLessonsQueryRequest()
    : IRequest<Result<List<SavedLessonDto>>>;