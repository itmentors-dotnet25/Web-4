using BookLibrary.Exceptions.Base;

namespace BookLibrary.Exceptions;

[ExceptionMetadata(
    ErrorMessage = "Запись не найдена",
    MessageCategory = "Ошибка поиска.",
    ErrorCode = 1444,
    StatusCode = StatusCodes.Status404NotFound,
    LogLevel = LogLevel.Warning,
    EmptyResponseBody = false
)]
public class BookNotFoundException(Dictionary<string, object?>? context = null) 
    : ModelNotFoundException("Book", context)
{
    public static BookNotFoundException ById(int id) => 
        new(new Dictionary<string, object?> { ["id"] = id });
    
    public static BookNotFoundException ByTitle(string title) => 
        new(new Dictionary<string, object?> { ["title"] = title });
    
    public static BookNotFoundException ByIsbn(string isbn) => 
        new(new Dictionary<string, object?> { ["isbn"] = isbn });
    
    public static BookNotFoundException ByAuthor(string author) => 
        new(new Dictionary<string, object?> { ["author"] = author });
    
    public static BookNotFoundException ByCriteria(Dictionary<string, object?> criteria) => 
        new(criteria);
}
