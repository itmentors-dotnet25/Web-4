namespace BookLibrary.Config;

public class ApplicationSettings
{
    public LoggingSettings Logging { get; set; } = new();
    public ExceptionHandlingSettings Exceptions { get; set; } = new();
}
