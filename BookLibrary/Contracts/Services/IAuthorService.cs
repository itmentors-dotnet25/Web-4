using BookLibrary.Models;

namespace BookLibrary.Contracts.Services;

public interface IAuthorService
{
    Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
}
