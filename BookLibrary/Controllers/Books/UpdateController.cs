using BookLibrary.Contracts.Services;
using BookLibrary.Data.Requests.Book;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Books;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
[Tags("Books")]
public class UpdateController(IBookService bookService) : ApiControllerBase
{
    /// <summary>
    /// Обновить книгу
    /// </summary>
    /// <param name="request">Данные книги</param>
    /// <param name="id"></param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная книга</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExecuteAsync(
        int id,
        UpdateBookRequest request,
        CancellationToken cancellationToken = default)
    {
        var updatedBook = await bookService.UpdateBookAsync(id, request, cancellationToken);
        
        return Success(updatedBook);
    }
}
