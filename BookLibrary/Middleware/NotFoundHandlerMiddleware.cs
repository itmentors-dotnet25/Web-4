using System.Text.Json;

namespace BookLibrary.Middleware;

public class NotFoundHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response is { StatusCode: 404, HasStarted: false })
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                success = false,
                message = "Route not found",
                path = context.Request.Path.Value,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}