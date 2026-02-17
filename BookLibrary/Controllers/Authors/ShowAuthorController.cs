using BookLibrary.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Authors;

[ApiController]
[Route("api/authors")]
[Produces("application/json")]
[Tags("Authors")]
public class ShowAuthorController(IAuthorService authorService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await authorService.GetAuthorByIdAsync(id, cancellationToken);
        
        return Success(author);
    }
}
