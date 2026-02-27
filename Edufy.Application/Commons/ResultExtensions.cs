using Edufy.Domain.Commons;
using Microsoft.AspNetCore.Mvc;

namespace Edufy.Application.Commons;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
        => new ObjectResult(result) { StatusCode = result.StatusCode };
}