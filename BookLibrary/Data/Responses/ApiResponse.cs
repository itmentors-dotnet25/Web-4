namespace BookLibrary.Data.Responses;

public class ApiResponseDto<T>(bool success, T? data, string message)
{
    public bool Success { get; } = success;
    public T? Data { get; } = data;
    public string Message { get; } = message;
}
