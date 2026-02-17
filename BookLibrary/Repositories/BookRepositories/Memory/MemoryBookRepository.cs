using BookLibrary.Contracts.Repositories;
using BookLibrary.Data.Requests.Book;
using BookLibrary.Data.Responses.Categories;
using BookLibrary.Storage;
using BookLibrary.Exceptions;
using BookLibrary.Models;
using BookLibrary.Specifications;
using BookLibrary.Specifications.Book;

namespace BookLibrary.Repositories.BookRepositories.Memory;

public class MemoryBookRepository(InMemoryStore store) : IBookRepository
{
    private readonly List<Book> _books = store.GetCollection<Book>();
    private readonly List<Author> _authors = store.GetCollection<Author>();
    private readonly List<Category> _categories = store.GetCollection<Category>();

    public Task<IEnumerable<Book>> GetAllAsync(BookFilterParams? filterParams = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var query = _books.AsQueryable();
        
        if (filterParams != null)
        {
            var specification = new MyBookSpecification(filterParams);
            query = query.ApplyMySpecification(specification);
            var result = query.ToList();
            
            ClearRelatedData(result);
            
            if (specification.IncludeStrings.Any())
            {
                LoadRelatedData(result, specification.IncludeStrings);
            }
        }
        
        return Task.FromResult<IEnumerable<Book>>(query.ToList());
    }

    public Task<Book> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        
        return book == null 
            ? throw BookNotFoundException.ById(id) 
            : Task.FromResult(book);
    }

    public Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        
        book.Id = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
        _books.Add(book);
        
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(book);
    }

    public Task<Book> UpdateAsync(int id, UpdateBookRequest data, CancellationToken cancellationToken = default)
    {
        var index = _books.FindIndex(b => b.Id == id);
    
        if (index == -1)
            throw BookNotFoundException.ById(id);
        
        var book = _books[index];
        data.ApplyTo(book);
    
        // Сохраняем оригинальный Id из параметра (на случай если во входящем объекте он другой)
        book.Id = id;
        book.UpdatedAt = DateTime.UtcNow;
        _books[index] = book;
    
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(book);
    }
    
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
    
        var index = _books.FindIndex(b => b.Id == id);
    
        if (index == -1)
            return Task.FromResult(false);
    
        _books.RemoveAt(index);
        return Task.FromResult(true);
    }
    
    public Task<IEnumerable<CategoryStatisticsDto>> GetCategoryStatisticsAsync(
        bool withBooks = false,
        CancellationToken cancellationToken = default)
    {
        var statistics = _books
            .GroupBy(b => b.CategoryId)
            .Select(g => {
                var category = _categories.FirstOrDefault(c => c.Id == g.Key);
                return new CategoryStatisticsDto(
                    category?.Name ?? "Без категории",
                    g.Count(),
                    (int)g.Average(b => (double)b.PublicationYear),
                    withBooks ? g.Select(b => new BookSummaryDto(b.Id, b.Title, b.PublicationYear)).ToList() : null
                );
            })
            .ToList();

        return Task.FromResult<IEnumerable<CategoryStatisticsDto>>(statistics);
    }
    
    private void LoadRelatedData(List<Book> books, IEnumerable<string> includes)
    {
        foreach (var include in includes)
        {
            switch (include)
            {
                case nameof(Book.Author):
                    foreach (var book in books)
                    {
                        book.Author = _authors.FirstOrDefault(a => a.Id == book.AuthorId);
                    }
                    break;

                case nameof(Book.Category):
                    foreach (var book in books)
                    {
                        book.Category = _categories.FirstOrDefault(c => c.Id == book.CategoryId);
                    }
                    break;

                // При добавлении новой связи нужно добавить:
                // case nameof(Book.Publisher):
                //     foreach (var book in books)
                //     {
                //         book.Publisher = _publishers.FirstOrDefault(p => p.Id == book.PublisherId);
                //     }
                //     break;
            }
        }
    }
    
    private void ClearRelatedData(List<Book> books)
    {
        foreach (var book in books)
        {
            book.Author = null;
            book.Category = null;
            // При добавлении новых связей нужно добавить сюда:
            // book.Publisher = null;
        }
    }
}
