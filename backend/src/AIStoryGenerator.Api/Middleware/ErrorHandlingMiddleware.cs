using Microsoft.AspNetCore.Http;
using System.Net;

namespace AIStoryGenerator.Api.Middleware;

/// <summary>
/// Middleware for error handling and structured error responses
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = "An error occurred while processing your request"
        };

        context.Response.StatusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        if (context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            response.Exception = exception.GetType().Name;
            response.Details = exception.Message;
            response.StackTrace = exception.StackTrace;
        }

        return context.Response.WriteAsJsonAsync(response);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string? Details { get; set; }
        public string? StackTrace { get; set; }
    }
}
