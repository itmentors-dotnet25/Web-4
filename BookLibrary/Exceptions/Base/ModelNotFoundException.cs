using System.Reflection;
using BookLibrary.Exceptions.Contracts;

namespace BookLibrary.Exceptions.Base;

[ExceptionMetadata(
    ErrorMessage = "Запись не найдена",
    MessageCategory = "Ошибка поиска.",
    ErrorCode = 404,
    StatusCode = StatusCodes.Status404NotFound,
    LogLevel = LogLevel.Warning,
    EmptyResponseBody = true
)]
public class ModelNotFoundException(string modelName, Dictionary<string, object?>? context = null) 
    : Exception
{
    public string ModelName { get; } = modelName;
    public Dictionary<string, object?>? Context { get; } = context;
}
