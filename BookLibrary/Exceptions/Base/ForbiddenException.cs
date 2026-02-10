namespace BookLibrary.Exceptions.Base;

[ExceptionMetadata(
    ErrorMessage = "У вас нет прав для выполнения этого действия",
    MessageCategory = "Ошибка доступа.",
    ErrorCode = 403,
    StatusCode = StatusCodes.Status403Forbidden,
    LogLevel = LogLevel.Information
)]
public class ForbiddenException(Dictionary<string, object?>? context = null) 
    : Exception()
{
    public Dictionary<string, object?>? Context { get; } = context;
}
