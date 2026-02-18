namespace Edufy.Domain.Common;

public record Result<T>(bool Success, int StatusCode, string? Message, T? Data)
{
    public static Result<T> Ok(T data, string? msg = null)
        => new(true, StatusCode.Status200OK, msg, data);

    public static Result<T> Created(T data, string? msg = null)
        => new(true, StatusCodes.Status201Created, msg, data);

    public static Result<T> Fail(int statusCode, string msg)
        => new(false, statusCode, msg, default);

    public static Result<T> BadRequest(string msg) => Fail(StatusCodes.Status400BadRequest, msg);
    public static Result<T> Unauthorized(string msg = "Unauthorized.") => Fail(StatusCodes.Status401Unauthorized, msg);
    public static Result<T> Forbidden(string msg = "Forbidden.") => Fail(StatusCodes.Status403Forbidden, msg);
    public static Result<T> NotFound(string msg = "Not found.") => Fail(StatusCodes.Status404NotFound, msg);
    public static Result<T> Conflict(string msg) => Fail(StatusCodes.Status409Conflict, msg);
    public static Result<T> ServerError(string msg = "Internal server error.") => Fail(StatusCodes.Status500InternalServerError, msg);
}
