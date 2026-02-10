using BookLibrary.Contracts.Services;
using BookLibrary.Specifications.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Books;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
[Tags("Books")]
public class ListBooksController(IBookService bookService) : ApiControllerBase
{
    /// <summary>
    /// Получить все книги
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromQuery] BookFilterParams filterParams,
        CancellationToken cancellationToken = default
        )
    {
        var books = await bookService.GetAllBooksAsync(filterParams, cancellationToken);
        return Success(books);
    }
}
