namespace BookLibrary.Data.Responses;

public class ApiRouteNotFoundResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Path { get; set; }
}
