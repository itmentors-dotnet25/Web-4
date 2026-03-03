using Microsoft.AspNetCore.Mvc;
using BookLibrary.Models;

namespace BookLibrary.Extensions
{
    /// <summary>
    /// Методы расширения для создания стандартизированных ответов
    /// </summary>
    public static class ApiResponseExtensions
    {
        /// <summary>
        /// Возвращает успешный ответ (200)
        /// </summary>
        public static IActionResult ApiOk<T>(this ControllerBase controller, T data, string? message = null)
        {
            return controller.Ok(new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Operation completed successfully"
            });
        }

        /// <summary>
        /// Возвращает успешный ответ без данных (204)
        /// </summary>
        public static IActionResult ApiNoContent(this ControllerBase controller, string? message = null)
        {
            return controller.Ok(new ApiResponse<object>
            {
                Success = true,
                Data = null,
                Message = message ?? "Operation completed successfully"
            });
        }

        /// <summary>
        /// Возвращает ошибку (400 по умолчанию)
        /// </summary>
        public static IActionResult ApiError(this ControllerBase controller, string message, int statusCode = StatusCodes.Status400BadRequest, object? data = null)
        {
            var response = new ApiResponse<object>
            {
                Success = false,
                Data = data,
                Message = message
            };

            return statusCode switch
            {
                StatusCodes.Status400BadRequest => controller.BadRequest(response),
                StatusCodes.Status404NotFound => controller.NotFound(response),
                StatusCodes.Status401Unauthorized => controller.Unauthorized(response),
                StatusCodes.Status403Forbidden => controller.StatusCode(StatusCodes.Status403Forbidden, response),
                StatusCodes.Status500InternalServerError => controller.StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => controller.StatusCode(statusCode, response)
            };
        }

        /// <summary>
        /// Возвращает ответ создания ресурса (201)
        /// </summary>
        public static IActionResult ApiCreated<T>(this ControllerBase controller, T data, string? location = null, string? message = null)
        {
            var response = new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Resource created successfully"
            };

            if (!string.IsNullOrEmpty(location))
            {
                return controller.Created(location, response);
            }

            return controller.StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// Возвращает ответ с пагинацией
        /// </summary>
        public static IActionResult ApiPaged<T>(
            this ControllerBase controller,
            List<T> data,
            int totalCount,
            int page,
            int pageSize,
            string? message = null)
        {
            var response = new PagedApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Operation completed successfully",
                Meta = new PaginationMeta
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                }
            };

            return controller.Ok(response);
        }
    }
}