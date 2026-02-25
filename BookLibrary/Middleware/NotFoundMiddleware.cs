using System.Text.Json;
using BookLibrary.Models;

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

        if (context.Response.StatusCode == 404)
        {
            _logger.LogWarning("Маршрут не найден: {Path}", context.Request.Path);

            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.ErrorResponse("Маршрут не найден");
            response.Data = new { path = context.Request.Path.ToString() };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}

public static class NotFoundMiddlewareExtensions
{
    public static IApplicationBuilder UseNotFoundMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<NotFoundMiddleware>();
    }
}