using BookLibrary.Models;
using BookLibrary.Requests;
using BookLibrary.Services;
using BookLibrary.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/[controller]")] // Например: api/books
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // ✅ Явный [HttpGet] без параметров
    [HttpGet] // GET: api/books
    public IActionResult GetAll([FromQuery] BookFilterRequest filter)
    {
        try
        {
            var books = _bookService.GetAll(filter);
            return this.ApiOk(books, "Books retrieved successfully");
        }
        catch (Exception ex)
        {
            return this.ApiError($"Failed to retrieve books: {ex.Message}", StatusCodes.Status500InternalServerError);
        }
    }

    // ✅ Явный [HttpGet] с параметром маршрута
    [HttpGet("{id}")] // GET: api/books/5
    public ActionResult<Book> GetBookById(int id)
    {
        var book = _bookService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        return book;
    }

    // ✅ Примеры других методов
    [HttpPost] // POST: api/books
    public ActionResult<Book> CreateBook(Book book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdBook = _bookService.CreateBook(book);
        return createdBook;
    }

    [HttpPut("{id}")] // PUT: api/books/5
    public IActionResult UpdateBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        var updated = _bookService.UpdateBook(id, book);

        return NoContent();
    }

    [HttpDelete("{id}")] // DELETE: api/books/5
    public IActionResult DeleteBook(int id)
    {
        var deleted = _bookService.DeleteBook(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}