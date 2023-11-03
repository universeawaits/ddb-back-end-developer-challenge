using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace HeroStats.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        string errorMessage;
        int statusCode;

        switch (exception)
        {
            case ValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                errorMessage = $"Bad request. {exception.Message}";
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                errorMessage = "Internal error occured";
                break;
        }

        await WriteResponseAsync(httpContext, errorMessage, statusCode);
    }

    private static async Task WriteResponseAsync(HttpContext httpContext, string errorMessage, int statusCode)
    {
        var paymentErrorDto = new { ErrorMessage = errorMessage };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await JsonSerializer.SerializeAsync(httpContext.Response.Body, paymentErrorDto);
    }
}
