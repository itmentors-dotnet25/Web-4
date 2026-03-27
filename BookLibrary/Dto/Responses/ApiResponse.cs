namespace BookLibrary.Dto.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; } = true;
    public T? Data { get; init; }
    public string Message { get; init; } = "Operation completed successfully.";
}

public class ApiResponse : ApiResponse<object?>
{
    public ApiResponse()
    {
        Success = false;
        Message = "An error occurred.";
    }
}