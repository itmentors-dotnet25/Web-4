using BookLibrary.Models;
using System.Net;
using System.Text.Json;

namespace BookLibrary.Middleware
{
    /// <summary>
    /// Обработчик несуществующих маршрутов (404)
    /// </summary>
    public class NotFoundHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isDevelopment;

        public NotFoundHandlingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _isDevelopment = environment.IsDevelopment();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Если ответ ещё не отправлен и статус 404 — обрабатываем как несуществующий маршрут
            if (context.Response.StatusCode == StatusCodes.Status404NotFound
                && !context.Response.HasStarted)
            {
                var traceId = Guid.NewGuid().ToString("N").Substring(0, 8);

                var response = new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Маршрут не найден",
                    TraceId = traceId,
                    Details = _isDevelopment
                        ? $"Запрошенный URL: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}"
                        : null
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = _isDevelopment
                }));
            }
        }
    }
}