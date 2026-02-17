using BookLibrary.Contracts.Repositories;
using BookLibrary.Exceptions.Base;
using BookLibrary.Models;
using BookLibrary.Specifications;
using BookLibrary.Specifications.Author;
using BookLibrary.Storage;

namespace BookLibrary.Repositories.AuthorRepositories.Memory;

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
            query = query.ApplyMySpecification(specification);
        }
        
        return Task.FromResult<IEnumerable<Author>>(query.ToList());
    }

    public Task<Author> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);

        return author == null 
            ? throw new ModelNotFoundException("Author") 
            : Task.FromResult(author);
    }
}
