namespace BookLibrary.Exceptions.Base;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ExceptionMetadataAttribute : Attribute
{
    public string MessageCategory { get; set; } = "Ошибка сервера.";
    public string ErrorMessage { get; set; } = "Произошла непредвиденная ошибка";
    public int ErrorCode { get; set; } = 500;
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
    public bool LogEnabled { get; set; } = true;
    public LogLevel LogLevel { get; set; } = LogLevel.Error;
    public bool EmptyResponseBody { get; set; } = false;
}
