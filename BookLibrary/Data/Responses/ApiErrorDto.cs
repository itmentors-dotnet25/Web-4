namespace BookLibrary.Data.Responses;

public class ApiErrorDto(string message, ErrorDetailDto error)
{
    public string Message { get; } = message;
    public ErrorDetailDto Error { get; } = error;
}
