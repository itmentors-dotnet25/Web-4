using BookLibrary.Models;
using BookLibrary.Specifications.Author;

namespace BookLibrary.Contracts.Repositories;

public interface IAuthorReadRepository
{
    /// <summary>
    /// Получить всех авторов с фильтрацией, сортировкой и пагинацией
    /// </summary>
    Task<IEnumerable<Author>> GetAllAsync(
        AuthorFilterParams? filterParams = null, 
        CancellationToken cancellationToken = default);
}
