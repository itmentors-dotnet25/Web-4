using BookLibrary.Data.Responses.Categories;

namespace BookLibrary.Contracts.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryStatisticsDto>> GetStatisticsAsync(
        bool withBooks = false, 
        CancellationToken cancellationToken = default);
}
