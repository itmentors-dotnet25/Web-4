using BookLibrary.Contracts;
using BookLibrary.Mapping;
using BookLibrary.Services;
using Microsoft.AspNetCore.Mvc;
namespace BookLibrary.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService, ILogger<BooksController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks([FromQuery] string? author = null, [FromQuery] string? sortBy = null)
    {
        logger.LogInformation("Запрос на получение всех книг. Параметры: author={Author}, sortBy={SortBy}", author,
            sortBy);

        var books = await bookService.GetAllBooksAsync(author, sortBy);
        var dto = books.Select(b => b.ToDto());

        return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(dto, "Книги получены успешно"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        logger.LogInformation("Запрос на получение книги с ID: {BookId}", id);

        var book = await bookService.GetBookByIdAsync(id);

        if (book == null)
        {
            logger.LogWarning("Книга с ID: {BookId} не найдена", id);
            return NotFound(ApiResponse<BookDto>.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse<BookDto>.SuccessResponse(book.ToDto(), "Книга получена успешно"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest? request)
    {
        logger.LogInformation("Запрос на создание книги. Title={Title}, ISBN={ISBN}", request?.Title, request?.ISBN);

        if (request == null)
        {
            logger.LogWarning("Получен пустой объект запроса на создание книги");
            return BadRequest(ApiResponse<BookDto>.ErrorResponse("Данные книги не могут быть пустыми"));
        }

        try
        {
            var createdBook = await bookService.CreateBookAsync(request.ToModel());

            return CreatedAtAction(
                nameof(GetBookById),
                new { id = createdBook.Id },
                ApiResponse<BookDto>.SuccessResponse(createdBook.ToDto(), "Книга успешно создана")
            );
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Конфликт при создании книги. ISBN={ISBN}", request.ISBN);
            return Conflict(ApiResponse<BookDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequest? request)
    {
        logger.LogInformation("Запрос на обновление книги с ID: {BookId}", id);

        if (request == null)
        {
            logger.LogWarning("Получен пустой объект запроса на обновление книги. Id={BookId}", id);
            return BadRequest(ApiResponse<BookDto>.ErrorResponse("Данные книги не могут быть пустыми"));
        }

        try
        {
            var updatedBook = await bookService.UpdateBookAsync(id, request.ToModel());

            if (updatedBook == null)
            {
                logger.LogWarning("Книга с ID: {BookId} не найдена для обновления", id);
                return NotFound(ApiResponse<BookDto>.ErrorResponse($"Книга с ID {id} не найдена"));
            }

            return Ok(ApiResponse<BookDto>.SuccessResponse(updatedBook.ToDto(), "Книга успешно обновлена"));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Конфликт при обновлении книги. Id={BookId} ISBN={ISBN}", id, request.ISBN);
            return Conflict(ApiResponse<BookDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        logger.LogInformation("Запрос на удаление книги с ID: {BookId}", id);

        var deleted = await bookService.DeleteBookAsync(id);

        if (!deleted)
        {
            logger.LogWarning("Книга с ID: {BookId} не найдена для удаления", id);
            return NotFound(ApiResponse.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse.SuccessResponse("Книга успешно удалена"));
    }
}