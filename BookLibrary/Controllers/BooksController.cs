using BookLibrary.Models;
using BookLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks([FromQuery] string? author = null, [FromQuery] string? sortBy = null)
    {
        _logger.LogInformation("Запрос на получение всех книг. Параметры: author={Author}, sortBy={SortBy}", author,
            sortBy);

        var books = await _bookService.GetAllBooksAsync(author, sortBy);

        return Ok(ApiResponse<IEnumerable<Book>>.SuccessResponse(books, "Книги получены успешно"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        _logger.LogInformation("Запрос на получение книги с ID: {BookId}", id);

        var book = await _bookService.GetBookByIdAsync(id);

        if (book == null)
        {
            _logger.LogWarning("Книга с ID: {BookId} не найдена", id);

            return NotFound(ApiResponse<Book>.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse<Book>.SuccessResponse(book, "Книга получена успешно"));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        _logger.LogInformation("Запрос на создание книги: {Title}", book?.Title);

        if (book == null)
        {
            _logger.LogWarning("Получен пустой объект книги");

            return BadRequest(ApiResponse<Book>.ErrorResponse("Данные книги не могут быть пустыми"));
        }

        var createdBook = await _bookService.CreateBookAsync(book);

        return CreatedAtAction(
            nameof(GetBookById),
            new { id = createdBook.Id },
            ApiResponse<Book>.SuccessResponse(createdBook, "Книга успешно создана")
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        _logger.LogInformation("Запрос на обновление книги с ID: {BookId}", id);
        
        var updatedBook = await _bookService.UpdateBookAsync(id, book);

        if (updatedBook == null)
        {
            _logger.LogWarning("Книга с ID: {BookId} не найдена для обновления", id);
            return NotFound(ApiResponse<Book>.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse<Book>.SuccessResponse(updatedBook, "Книга успешно обновлена"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        _logger.LogInformation("Запрос на удаление книги с ID: {BookId}", id);
        
        var deleted = await _bookService.DeleteBookAsync(id);

        if (!deleted)
        {
            _logger.LogWarning("Книга с ID: {BookId} не найдена для удаления", id);
            
            return NotFound(ApiResponse.ErrorResponse($"Книга с ID {id} не найдена"));
        }

        return Ok(ApiResponse.SuccessResponse("Книга успешно удалена"));
    }
}