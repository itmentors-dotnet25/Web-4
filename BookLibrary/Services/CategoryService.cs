using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Data.Responses.Categories;

namespace BookLibrary.Services;

public class CategoryService(IBookRepository bookRepository) : ICategoryService
{
    public async Task<IEnumerable<CategoryStatisticsDto>> GetStatisticsAsync(
        bool withBooks = false,
        CancellationToken cancellationToken = default)
    {
        return await bookRepository.GetCategoryStatisticsAsync(withBooks, cancellationToken);
    }
}
