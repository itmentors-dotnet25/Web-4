using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Specifications;
using BookLibrary.Data.Requests.Book;
using BookLibrary.Data.Responses.Categories;
using BookLibrary.Database.Context;
using BookLibrary.Exceptions;
using BookLibrary.Models;
using BookLibrary.Specifications.Book;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories.BookRepositories.EFCore;

public class EfCoreBookRepository(AppDbContext context) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllAsync(
        BookFilterParams? filterParams = null,
        CancellationToken cancellationToken = default)
    {
        // Создаем спецификацию
        var specification = new MyBookSpecification(filterParams ?? new BookFilterParams());
        
        // Применяем спецификацию
        var query = ApplySpecification(specification);
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Book> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        return book ?? throw BookNotFoundException.ById(id);
    }

    public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        book.CreatedAt = DateTime.UtcNow;
        book.UpdatedAt = DateTime.UtcNow;
        
        context.Books.Add(book);
        await context.SaveChangesAsync(cancellationToken);
        
        // Загружаем навигационные свойства
        await context.Entry(book)
            .Reference(b => b.Author)
            .LoadAsync(cancellationToken);
        
        await context.Entry(book)
            .Reference(b => b.Category)
            .LoadAsync(cancellationToken);
        
        return book;
    }

    public async Task<Book> UpdateAsync(int id, UpdateBookRequest data, CancellationToken cancellationToken = default)
    {
        var existingBook = await GetByIdAsync(id, cancellationToken);
        
        data.ApplyTo(existingBook);
        existingBook.UpdatedAt = DateTime.UtcNow;
        
        context.Books.Update(existingBook);
        await context.SaveChangesAsync(cancellationToken);
        
        return existingBook;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await context.Books
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (book == null)
            return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
    
    public async Task<IEnumerable<CategoryStatisticsDto>> GetCategoryStatisticsAsync(
        bool withBooks = false,
        CancellationToken cancellationToken = default)
    {
        var result = await context.Books
            .Where(b => b.Category != null) // Защита от нарушения FK
            .GroupBy(b => b.Category!.Name)
            .ToListAsync(cancellationToken);
        return await context.Books
            .Where(b => b.Category != null) // Защита от нарушения FK
            .GroupBy(b => b.Category!.Name)
            .Select(g => new CategoryStatisticsDto(
                g.Key, 
                g.Count(), 
                (int)g.Average(b => (double)b.PublicationYear),
                // в зависимости от параметра вставляем книги в ответ или нет
                withBooks ? g.Select(b => new BookSummaryDto(b.Id, b.Title, b.PublicationYear)).ToList() : null
            ))
            .ToListAsync(cancellationToken);
    }


    /// <summary>
    /// Применяет спецификацию к запросу
    /// </summary>
    private IQueryable<Book> ApplySpecification(IMySpecification<Book> spec)
    {
        var query = context.Books.AsQueryable();

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
