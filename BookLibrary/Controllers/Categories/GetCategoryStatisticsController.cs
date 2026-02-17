using BookLibrary.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Categories;

[ApiController]
[Route("api/categories/statistics")]
[Produces("application/json")]
[Tags("Categories")]
public class GetCategoryStatisticsController(ICategoryService categoryService) : ApiControllerBase
{
    /// <summary>
    /// Получить статистику по категориям:
    /// - Название категории
    /// - Количество книг
    /// - Средний год публикации
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromQuery] bool withBooks = false,
        CancellationToken cancellationToken = default)
    {
        var statistics = await categoryService.GetStatisticsAsync(withBooks, cancellationToken);
        
        return Success(statistics);
    }
}
