namespace BookLibrary.Config;

public class ExceptionHandlingSettings
{
    public string? MessageCategory { get; set; }
    public string? ErrorMessage { get; set; }
    public int? ErrorCode { get; set; }
    public int? StatusCode { get; set; }
    public bool? LogEnabled { get; set; }
    public LogLevel? LogLevel { get; set; }
    public bool? EmptyResponseBody { get; set; }
}
