using Edufy.Domain.Commons;
using Edufy.Domain.DTOs.ApplicationDTOs;
using MediatR;

namespace Edufy.Application.CQRS.Queries.Requests;

public record GetMyApplicationsQueryRequest
    : IRequest<Result<List<MyApplicationDto>>>;