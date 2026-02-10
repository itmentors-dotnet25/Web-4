using Ardalis.Specification.EntityFrameworkCore;
using BookLibrary.Contracts.Repositories;
using BookLibrary.Models;
using BookLibrary.Specifications.Author;
using BookLibrary.Storage;

namespace BookLibrary.Repositories.AuthorRepositories;

/// <summary>
/// In-Memory репозиторий для авторов.
/// Использует Ardalis.Specification для фильтрации
/// </summary>
public class MemoryAuthorReadRepository(InMemoryStore store) : IAuthorReadRepository
{
    private readonly List<Author> _authors = store.GetCollection<Author>();

    public Task<IEnumerable<Author>> GetAllAsync(AuthorFilterParams? filterParams = null, CancellationToken cancellationToken = default)
    {
        var query = _authors.AsQueryable();
        
        // Применяем спецификацию Ardalis
        if (filterParams != null)
        {
            var specification = new ArdalisAuthorSpecification(filterParams);
            query = query.WithSpecification(specification);
        }
        
        return Task.FromResult<IEnumerable<Author>>(query.ToList());
    }
}
