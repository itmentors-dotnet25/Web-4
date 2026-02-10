using BookLibrary.Contracts.Services;
using BookLibrary.Exceptions.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Books;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
[Tags("Books")]
public class ShowBookController(IBookService bookService) : ApiControllerBase
{
    /// <summary>
    /// Получить книгу по ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await bookService.GetBookByIdAsync(id, cancellationToken);
        
        return Success(book);
    }
}
