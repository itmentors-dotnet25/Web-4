using BookLibrary.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace BookLibrary.Middleware;

/// <summary>
/// Глобальный обработчик исключений
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly bool _isDevelopment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _isDevelopment = environment.IsDevelopment();
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Генерируем уникальный идентификатор для трассировки
        var traceId = Guid.NewGuid().ToString("N").Substring(0, 8);
        var statusCode = GetStatusCode(exception);
        var message = GetMessage(exception, statusCode);

        // Логируем ошибку
        _logger.LogError(exception, "Unhandled exception [TraceId: {TraceId}]", traceId);

        // Формируем ответ
        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            TraceId = traceId,
            Details = _isDevelopment ? exception.ToString() : null
        };

        // Особая обработка для ошибок валидации ModelState
        if (exception is ValidationException validationException)
        {
            response.Errors = validationException.Errors;
        }
        else if (context.Features.Get<IExceptionHandlerFeature>()?.Error is ValidationException ve)
        {
            response.Errors = ve.Errors;
        }

        // Устанавливаем статус и возвращаем JSON
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _isDevelopment
        }));
    }

    private int GetStatusCode(Exception exception) => exception switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        ForbiddenException => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };

    private string GetMessage(Exception exception, int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Некорректный запрос",
        StatusCodes.Status401Unauthorized => "Необходима аутентификация",
        StatusCodes.Status403Forbidden => "Доступ запрещён",
        StatusCodes.Status404NotFound => "Ресурс не найден",
        StatusCodes.Status500InternalServerError => "Внутренняя ошибка сервера",
        _ => "Произошла непредвиденная ошибка"
    };
}

/// <summary>
/// Исключение для ошибок валидации с деталями
/// </summary>
public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }
}

/// <summary>
/// Исключение для ошибок доступа
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}