namespace BookLibrary.Contracts;

public class ApiResponse<T>(bool success, T? data, string? message)
{
    public bool Success { get; set; } = success;
    public T? Data { get; set; } = data;
    public string? Message { get; set; } = message;

    public static ApiResponse<T> SuccessResponse(T? data, string? message = null)
        => new(true, data, message ?? "Операция выполнена успешно");

    public static ApiResponse<T> ErrorResponse(string? message, T? data = default)
        => new(false, data, message ?? "Произошла ошибка");
}

public class ApiResponse(bool success, string? message) : ApiResponse<object>(success, null, message)
{
    public static ApiResponse SuccessResponse(string? message = null)
        => new(true, message ?? "Операция выполнена успешно");

    public static ApiResponse ErrorResponse(string? message)
        => new(false, message ?? "Произошла ошибка");
}