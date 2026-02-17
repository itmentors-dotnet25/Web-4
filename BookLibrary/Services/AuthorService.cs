using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Models;

namespace BookLibrary.Services;

public class AuthorService(IAuthorReadRepository readRepository) : IAuthorService
{
    public async Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = readRepository.GetByIdAsync(id, cancellationToken);
        
        return await result;
    }
}
