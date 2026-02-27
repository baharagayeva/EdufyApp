namespace Edufy.Domain.Commons;

public record Result<T>(bool Success, int StatusCode, string? Message, T? Data)
{
    public static Result<T> Ok(T data, string? msg = null)
        => new(true, HttpStatus.Ok, msg, data);

    public static Result<T> Created(T data, string? msg = null)
        => new(true, HttpStatus.Created, msg, data);

    public static Result<T> Fail(int statusCode, string msg)
        => new(false, statusCode, msg, default);

    public static Result<T> BadRequest(string msg) => Fail(HttpStatus.BadRequest, msg);
    public static Result<T> Unauthorized(string msg = "Unauthorized.") => Fail(HttpStatus.Unauthorized, msg);
    public static Result<T> Forbidden(string msg = "Forbidden.") => Fail(HttpStatus.Forbidden, msg);
    public static Result<T> NotFound(string msg = "Not found.") => Fail(HttpStatus.NotFound, msg);
    public static Result<T> Conflict(string msg) => Fail(HttpStatus.Conflict, msg);
    public static Result<T> ServerError(string msg = "Internal server error.") => Fail(HttpStatus.ServerError, msg);
}