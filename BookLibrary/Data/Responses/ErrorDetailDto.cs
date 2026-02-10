namespace BookLibrary.Data.Responses;

public class ErrorDetailDto(int code, string message)
{
    public int Code { get; } = code;
    public string Message { get; } = message;
    
    // Опционально: детали исключения
    public Dictionary<string, object?>? Details { get; init; }
}
