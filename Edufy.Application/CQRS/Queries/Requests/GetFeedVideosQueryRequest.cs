using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.VideoCardDTOs;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public sealed record GetFeedVideosQueryRequest(
    string? Search,
    int Page,
    int PageSize
) : IRequest<Result<List<FeedVideoCardDto>>>;