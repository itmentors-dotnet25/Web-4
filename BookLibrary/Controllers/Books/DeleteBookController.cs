using BookLibrary.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Books;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
[Tags("Books")]
public class DeleteBookController(IBookService bookService) : ApiControllerBase
{
    /// <summary>
    /// Удалить книгу по ID
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await bookService.DeleteBookAsync(id, cancellationToken);

        return result 
            ? Success(new {result}) 
            : Fail(new {result});
    }
}
