namespace Edufy.Application.CQRS.Commands.Responses;

public record ApplyCommandResponse(
    int ApplicationId,
    string Message     // "Müraciət uğurla göndərildi"
);