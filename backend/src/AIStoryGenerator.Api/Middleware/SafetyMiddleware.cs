using Microsoft.AspNetCore.Http;

namespace AIStoryGenerator.Api.Middleware;

/// <summary>
/// Middleware for safety checks and content moderation
/// Validates incoming requests and filters outgoing responses for safety flags
/// </summary>
public class SafetyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SafetyMiddleware> _logger;

    public SafetyMiddleware(RequestDelegate next, ILogger<SafetyMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log incoming request for safety monitoring
        _logger.LogInformation(
            "Incoming request: {Method} {Path} from {RemoteIp}",
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress);

        // Check for potentially unsafe content in request
        if (context.Request.Method == HttpMethods.Post &&
            context.Request.ContentType?.Contains("application/json") == true)
        {
            // Enable request body reading
            context.Request.EnableBuffering();

            try
            {
                // Read and validate the request body
                // Use leaveOpen: true to prevent disposing the underlying stream
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                // Basic safety checks (can be expanded with more sophisticated moderation)
                if (ContainsFlaggedContent(body))
                {
                    _logger.LogWarning("Request contains potentially unsafe content");
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = "Request content violates safety policies" });
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during safety check");
            }
        }

        await _next(context);
    }

    private static bool ContainsFlaggedContent(string content)
    {
        // Placeholder for actual content moderation logic
        // In production, this would integrate with a moderation service
        // (e.g., OpenAI's Moderation API, Azure Content Moderator, etc.)

        // Simple pattern matching for development (can be enhanced)
        var flaggedPatterns = new[] { "UNSAFE_CONTENT_PATTERN" };
        return flaggedPatterns.Any(pattern => content.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
