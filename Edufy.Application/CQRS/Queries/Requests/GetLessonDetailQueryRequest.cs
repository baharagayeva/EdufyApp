using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.VideoCardDTOs;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetLessonDetailQueryRequest(int LessonId)
    : IRequest<Result<FeedVideoCardDto>>;