using BookLibrary.Models;
using BookLibrary.Specifications.Author;

namespace BookLibrary.Contracts.Repositories;

public interface IAuthorReadRepository
{
    Task<IEnumerable<Author>> GetAllAsync(
        AuthorFilterParams? filterParams = null, 
        CancellationToken cancellationToken = default);
    Task<Author> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
