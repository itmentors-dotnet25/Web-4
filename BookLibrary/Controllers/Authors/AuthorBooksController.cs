using BookLibrary.Contracts.Services;
using BookLibrary.Specifications.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Authors;

[ApiController]
[Route("api/authors/{id:int}/books")]
[Produces("application/json")]
[Tags("Authors")]
public class AuthorBooksController(
    IBookService bookService,
    IAuthorService authorService
    ) : ApiControllerBase
{
    /// <summary>
    /// Получить все книги автора с возможностью фильтрации и включения связей
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromRoute] int id,
        [FromQuery] BookFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var author = await authorService.GetAuthorByIdAsync(id, cancellationToken);
        filterParams.AuthorId = author.Id;

        var books = await bookService.GetAllBooksAsync(filterParams, cancellationToken);
        
        return Success(books);
    }
}
