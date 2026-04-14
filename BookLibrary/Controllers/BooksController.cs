using BookLibrary.Contracts;
using BookLibrary.Data;
using BookLibrary.Mapping;
using BookLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService, ILogger<BooksController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks([FromQuery] string? author = null, [FromQuery] string? sortBy = null)
    {
        logger.LogInformation(
            "Запрос на получение всех книг. author={Author}, sortBy={SortBy}",
            author,
            sortBy);

        var books = await bookService.GetAllBooksAsync(author, sortBy);
        var response = books.Select(book => book.ToDto());

        return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(response, "Книги получены успешно"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        logger.LogInformation("Запрос на получение книги с ID: {BookId}", id);

        var book = await bookService.GetBookByIdAsync(id);

        if (book is null)
        {
            logger.LogWarning("Книга с ID {BookId} не найдена", id);

            return NotFound(ApiResponse<BookDto>.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse<BookDto>.SuccessResponse(book.ToDto(), "Книга получена успешно"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest? request)
    {
        logger.LogInformation("Запрос на создание книги");

        if (request is null)
        {
            return BadRequest(ApiResponse<BookDto>.ErrorResponse("Данные книги не могут быть пустыми"));
        }

        try
        {
            var createdBook = await bookService.CreateBookAsync(request.ToModel());

            return CreatedAtAction(
                nameof(GetBookById),
                new { id = createdBook.Id },
                ApiResponse<BookDto>.SuccessResponse(createdBook.ToDto(), "Книга успешно создана"));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Ошибка при создании книги");

            return Conflict(ApiResponse<BookDto>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequest? request)
    {
        logger.LogInformation("Запрос на обновление книги с ID: {BookId}", id);

        if (request is null)
        {
            return BadRequest(ApiResponse<BookDto>.ErrorResponse("Данные книги не могут быть пустыми"));
        }

        try
        {
            var updatedBook = await bookService.UpdateBookAsync(id, request.ToModel());

            if (updatedBook is null)
            {
                return NotFound(ApiResponse<BookDto>.ErrorResponse($"Книга с ID {id} не найдена"));
            }

            return Ok(ApiResponse<BookDto>.SuccessResponse(updatedBook.ToDto(), "Книга успешно обновлена"));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Ошибка при обновлении книги");

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
            return NotFound(ApiResponse.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse.SuccessResponse("Книга успешно удалена"));
    }

    [HttpGet("with-details")]
    public async Task<IActionResult> GetBooksWithDetails()
    {
        logger.LogInformation("Запрос на получение книг с детализацией");

        var books = await bookService.GetBooksWithDetailsAsync();
        var response = books.Select(book => book.ToDto());

        return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(
            response,
            "Книги с детализацией получены успешно"));
    }

    [HttpGet("authors/{authorId:int}/books")]
    public async Task<IActionResult> GetBooksByAuthor(int authorId)
    {
        logger.LogInformation("Запрос на получение книг автора с ID: {AuthorId}", authorId);

        var books = await bookService.GetBooksByAuthorIdAsync(authorId);

        if (!books.Any())
        {
            var authorExists = await AuthorExists(authorId);

            if (!authorExists)
            {
                return NotFound(
                    ApiResponse<IEnumerable<BookDto>>.ErrorResponse($"Автор с ID {authorId} не найден"));
            }
        }

        var response = books.Select(book => book.ToDto());

        return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(
            response,
            $"Книги автора с ID {authorId} получены успешно"));
    }

    [HttpGet("categories/statistics")]
    public async Task<IActionResult> GetCategoryStatistics()
    {
        logger.LogInformation("Запрос на получение статистики по категориям");

        var stats = await bookService.GetCategoryStatisticsAsync();

        return Ok(ApiResponse<IEnumerable<CategoryStatsDto>>.SuccessResponse(
            stats,
            "Статистика по категориям получена успешно"));
    }

    private async Task<bool> AuthorExists(int authorId)
    {
        await using var scope = HttpContext.RequestServices.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Authors.AnyAsync(author => author.Id == authorId);
    }
}