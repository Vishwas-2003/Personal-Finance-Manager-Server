using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Common.Models.Api;

namespace WebApp.Api.Infrastructure;

public static class ControllerExceptionHandler
{
    public static IActionResult Handle(Exception exception, bool treatUnauthorizedAsSessionExpired = false)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            var result = TryMap(current, treatUnauthorizedAsSessionExpired);
            if (result is not null)
            {
                return result;
            }
        }

        return InternalError();
    }

    private static IActionResult? TryMap(Exception exception, bool treatUnauthorizedAsSessionExpired)
    {
        return exception switch
        {
            UnauthorizedAccessException when treatUnauthorizedAsSessionExpired => SessionExpired(),
            UnauthorizedAccessException unauthorized => Unauthorized(unauthorized.Message),
            InvalidOperationException invalidOperation => BadRequest(invalidOperation.Message),
            ArgumentException argument => BadRequest(argument.Message),
            KeyNotFoundException notFound => NotFound(notFound.Message),
            DbUpdateConcurrencyException => Conflict("The record was modified by another process. Please refresh and try again."),
            DbUpdateException => BadRequest("A database error occurred while processing your request."),
            _ => null
        };
    }

    public static IActionResult BadRequest(string message) =>
        StatusCode(StatusCodes.Status400BadRequest, ApiErrorCodes.BadRequest, message);

    public static IActionResult Unauthorized(string message) =>
        StatusCode(StatusCodes.Status401Unauthorized, ApiErrorCodes.Unauthorized, message);

    public static IActionResult NotFound(string message) =>
        StatusCode(StatusCodes.Status404NotFound, ApiErrorCodes.NotFound, message);

    public static IActionResult Conflict(string message) =>
        StatusCode(StatusCodes.Status409Conflict, ApiErrorCodes.Conflict, message);

    public static IActionResult SessionExpired() =>
        StatusCode(
            StatusCodes.Status401Unauthorized,
            ApiErrorCodes.SessionExpired,
            SessionExpiredResponseWriter.DefaultMessage);

    public static IActionResult InternalError() =>
        StatusCode(
            StatusCodes.Status500InternalServerError,
            ApiErrorCodes.InternalError,
            "An unexpected error occurred. Please try again later.");

    private static ObjectResult StatusCode(int statusCode, string code, string message) =>
        new(new ApiErrorResponse { Code = code, Message = message }) { StatusCode = statusCode };
}
