using System.Text.Json;

namespace BookLibrary.Middleware;

public class NotFoundMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<NotFoundMiddleware> _logger;

    public NotFoundMiddleware(RequestDelegate next, ILogger<NotFoundMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.HasStarted) return;

        if (context.Response.StatusCode == StatusCodes.Status404NotFound && context.GetEndpoint() == null)
        {
            _logger.LogWarning("Маршрут не найден: {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";

            var payload = new
            {
                success = false,
                message = "Маршрут не найден",
                path = context.Request.Path.ToString(),
                timestamp = DateTimeOffset.UtcNow.ToString("O")
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}

public static class NotFoundMiddlewareExtensions
{
    public static IApplicationBuilder UseNotFoundMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<NotFoundMiddleware>();
}