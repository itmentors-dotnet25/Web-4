using Microsoft.AspNetCore.Mvc;
using BookLibrary.Models;
using System.Text.Json;

namespace BookLibrary.Results
{
    /// <summary>
    /// Успешный результат с данными
    /// </summary>
    public class ApiOkResult<T> : IActionResult
    {
        private readonly ApiResponse<T> _response;

        public ApiOkResult(T data, string? message = null, int? statusCode = null)
        {
            _response = ApiResponse<T>.SuccessResponse(data, message);
            StatusCode = statusCode ?? StatusCodes.Status200OK;
        }

        public int StatusCode { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = StatusCode;

            var json = JsonSerializer.Serialize(_response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            await response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Успешный результат без данных
    /// </summary>
    public class ApiNoContentResult : IActionResult
    {
        private readonly ApiResponse<object> _response;

        public ApiNoContentResult(string? message = null)
        {
            _response = ApiResponse<object>.EmptySuccess(message ?? "Operation completed successfully");
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status204NoContent;

            var json = JsonSerializer.Serialize(_response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            await response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Результат с ошибкой
    /// </summary>
    public class ApiErrorResult : IActionResult
    {
        private readonly ApiResponse<object> _response;
        public int StatusCode { get; }

        public ApiErrorResult(string message, int statusCode = StatusCodes.Status400BadRequest, object? data = null)
        {
            _response = ApiResponse<object>.ErrorResponse(message, data);
            StatusCode = statusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = StatusCode;

            var json = JsonSerializer.Serialize(_response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            await response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Результат создания ресурса (201)
    /// </summary>
    public class ApiCreatedResult<T> : IActionResult
    {
        private readonly ApiResponse<T> _response;
        private readonly string? _location;

        public ApiCreatedResult(T data, string? location = null, string? message = null)
        {
            _response = ApiResponse<T>.SuccessResponse(data, message ?? "Resource created successfully");
            _location = location;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status201Created;

            if (!string.IsNullOrEmpty(_location))
            {
                response.Headers.Location = _location;
            }

            var json = JsonSerializer.Serialize(_response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });

            await response.WriteAsync(json);
        }
    }
}