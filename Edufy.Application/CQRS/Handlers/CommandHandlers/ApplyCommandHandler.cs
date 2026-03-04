using System.Text.RegularExpressions;
using Edufy.Application.CQRS.Commands.Requests;
using Edufy.Application.CQRS.Commands.Responses;
using Edufy.Domain.Abstractions;
using Edufy.Domain.Commons;
using Edufy.Domain.Enums;
using Edufy.SqlServer.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Edufy.Application.CQRS.Handlers.CommandHandlers;

public sealed class ApplyCommandHandler(EdufyDbContext db, ICurrentUser currentUser)
    : IRequestHandler<ApplyCommandRequest, Result<ApplyCommandResponse>>
{
    private static readonly Regex PhoneRegex =
        new(@"^\+?[0-9\s\-\(\)]{7,20}$", RegexOptions.Compiled);

    public async Task<Result<ApplyCommandResponse>> Handle(
        ApplyCommandRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
            return Result<ApplyCommandResponse>.BadRequest("Please enter a first name.");

        if (string.IsNullOrWhiteSpace(request.LastName))
            return Result<ApplyCommandResponse>.BadRequest("Please enter a last name.");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber) || !PhoneRegex.IsMatch(request.PhoneNumber))
            return Result<ApplyCommandResponse>.BadRequest("Please enter a proper phone number.");

        var programExists = await db.Programs
            .AnyAsync(x => x.Id == request.ProgramId, ct);

        if (!programExists)
            return Result<ApplyCommandResponse>.NotFound("Program not found.");

        var alreadyApplied = await db.Applications
            .AnyAsync(x => x.UserId == currentUser.UserId && x.ProgramId == request.ProgramId, ct);

        if (alreadyApplied)
            return Result<ApplyCommandResponse>.Conflict("You have already applied this program.");

        var application = new Domain.Entities.Application
        {
            UserId      = currentUser.UserId,
            ProgramId   = request.ProgramId,
            FirstName   = request.FirstName.Trim(),
            LastName    = request.LastName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Status      = ApplicationStatus.Pending,
            AppliedAt   = DateTime.UtcNow
        };

        db.Applications.Add(application);
        await db.SaveChangesAsync(ct);

        return Result<ApplyCommandResponse>.Ok(new ApplyCommandResponse(
            ApplicationId: application.Id,
            Message: "Applied successfully!"
        ));
    }
}