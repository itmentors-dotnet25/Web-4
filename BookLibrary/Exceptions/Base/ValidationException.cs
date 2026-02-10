using BookLibrary.Exceptions.Contracts;

namespace BookLibrary.Exceptions.Base;

[ExceptionMetadata(
    MessageCategory = "Указанные данные были неверными.",
    ErrorCode = 422,
    StatusCode = StatusCodes.Status422UnprocessableEntity,
    LogLevel = LogLevel.Warning
)]
public class ValidationException(Dictionary<string, List<string>> errors) 
    : Exception()
{
    public Dictionary<string, List<string>> Errors { get; } = errors;
}
