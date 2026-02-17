using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Specifications;
using BookLibrary.Database.Context;
using BookLibrary.Exceptions.Base;
using BookLibrary.Models;
using BookLibrary.Specifications.Author;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories.AuthorRepositories.EFCore;

public class EfCoreAuthorReadRepository(AppDbContext context) : IAuthorReadRepository
{
    public async Task<IEnumerable<Author>> GetAllAsync(AuthorFilterParams? filterParams = null, CancellationToken cancellationToken = default)
    {
        // Создаем спецификацию
        var specification = new ArdalisAuthorSpecification(filterParams ?? new AuthorFilterParams());
        
        // Применяем спецификацию
        var query = ApplySpecification(specification);
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Author> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var author = await context.Authors
            .Include(b => b.Books)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        return author ?? throw new ModelNotFoundException("Author");
    }
    
    private IQueryable<Author> ApplySpecification(IMySpecification<Author> spec)
    {
        var query = context.Authors.AsQueryable();

        // Применяем Include (выражения)
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Применяем Include (строки)
        query = spec.IncludeStrings.Aggregate(query, (current, includeString) => current.Include(includeString));

        // Применяем критерии фильтрации
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // Применяем сортировку
        if (spec.OrderBy.Count != 0)
        {
            query = spec.OrderBy.Aggregate(query, (current, orderBy) => current.OrderBy(orderBy));
        }

        if (spec.OrderByDescending.Count != 0)
        {
            query = spec.OrderByDescending.Aggregate(query, (current, orderByDesc) => current.OrderByDescending(orderByDesc));
        }

        // Если нет сортировки, применяем сортировку по умолчанию
        if (spec.OrderBy.Count == 0 && spec.OrderByDescending.Count == 0)
        {
            query = query.OrderBy(b => b.Id);
        }

        // Применяем пагинацию
        if (spec is { IsPagingEnabled: true, Skip: not null, Take: not null })
        {
            query = query.Skip(spec.Skip.Value).Take(spec.Take.Value);
        }

        return query;
    }
}
