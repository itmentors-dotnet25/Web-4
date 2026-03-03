namespace BookLibrary.Models
{
    /// <summary>
    /// Стандартизированный ответ для успешных операций
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Успешность операции
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Данные ответа
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Сообщение для клиента
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Дополнительные метаданные (опционально)
        /// </summary>
        public object? Meta { get; set; }

        /// <summary>
        /// Создать успешный ответ
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string? message = null, object? meta = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Operation completed successfully",
                Meta = meta
            };
        }

        /// <summary>
        /// Создать пустой успешный ответ
        /// </summary>
        public static ApiResponse<object> EmptySuccess(string? message = null)
        {
            return new ApiResponse<object>
            {
                Success = true,
                Data = null,
                Message = message ?? "Operation completed successfully"
            };
        }

        /// <summary>
        /// Создать ответ с ошибкой
        /// </summary>
        public static ApiResponse<object> ErrorResponse(string message, object? data = null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Data = data,
                Message = message
            };
        }
    }

    /// <summary>
    /// Стандартизированный ответ для пагинации
    /// </summary>
    public class PagedApiResponse<T>
    {
        public bool Success { get; set; }
        public List<T> Data { get; set; } = new();
        public string? Message { get; set; }
        public PaginationMeta Meta { get; set; } = new();

        public static PagedApiResponse<T> SuccessResponse(
            List<T> data,
            int totalCount,
            int page,
            int pageSize,
            string? message = null)
        {
            return new PagedApiResponse<T>
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
        }
    }

    /// <summary>
    /// Метаданные пагинации
    /// </summary>
    public class PaginationMeta
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}