using BookLibrary.Contracts.Services;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Books;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
[Tags("Books")]
public class StoreBookController(IBookService bookService)
    : ApiControllerBase
{
    /// <summary>
    /// Добавить новую книгу
    /// </summary>
    /// <param name="book">Данные книги</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная книга</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromBody] Book book,
        CancellationToken cancellationToken = default)
    {
        var createdBook = await bookService.CreateBookAsync(book, cancellationToken);

        return Created(createdBook);
    }
}
