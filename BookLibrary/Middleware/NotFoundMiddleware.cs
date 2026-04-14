using System.Text.Json;

namespace BookLibrary.Middleware;

public class NotFoundMiddleware(RequestDelegate next, ILogger<NotFoundMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.HasStarted)
        {
            return;
        }

        if (context.Response.StatusCode == StatusCodes.Status404NotFound && context.GetEndpoint() is null)
        {
            logger.LogWarning("Route not found: {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";

            var payload = new
            {
                success = false,
                message = "Route not found",
                path = context.Request.Path.ToString(),
                timestamp = DateTime.UtcNow.ToString("O")
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