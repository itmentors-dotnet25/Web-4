// BookLibrary/Controllers/Authors/ListAuthorsController.cs
using BookLibrary.Contracts.Repositories;
using BookLibrary.Specifications.Author;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Authors;

[ApiController]
[Route("api/authors")]
[Produces("application/json")]
[Tags("Authors")]
public class ListAuthorsController(IAuthorReadRepository authorRepository) : ApiControllerBase()
{
    /// <summary>
    /// Получить список авторов с фильтрацией, сортировкой и пагинацией
    /// </summary>
    /// <param name="filterParams">Параметры фильтрации</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список авторов</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromQuery] AuthorFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var authors = await authorRepository.GetAllAsync(filterParams, cancellationToken);
            
        return Success(authors);
    }
}
