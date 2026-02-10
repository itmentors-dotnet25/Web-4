using System.Text.Json;
using BookLibrary.Data.Responses;

namespace BookLibrary.Middleware;

public class RouteNotFoundMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Сначала пробуем выполнить следующий middleware
        await next(context);

        // Если ответ 404 и ещё не записан (не было исключения)
        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 404;
            
            var response = new ApiRouteNotFoundResponse
            {
                Success = false,
                Message = "Route not found",
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };
            
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
