using BookLibrary.Contracts.Repositories;
using BookLibrary.Storage;
using BookLibrary.Exceptions;
using BookLibrary.Models;
using BookLibrary.Specifications;
using BookLibrary.Specifications.Book;

namespace BookLibrary.Repositories.BookRepositories;

public class MemoryBookRepository(InMemoryStore store) : IBookRepository
{
    private readonly List<Book> _books = store.GetCollection<Book>();

    public Task<IEnumerable<Book>> GetAllAsync(BookFilterParams? filterParams = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var query = _books.AsQueryable();
        
        if (filterParams != null)
        {
            var specification = new MyBookSpecification(filterParams);
            query = query.ApplyMySpecification(specification);
        }
        
        return Task.FromResult<IEnumerable<Book>>(query.ToList());
    }

    public Task<Book> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        
        if (book == null)
        {
            throw BookNotFoundException.ById(id);
        }
        
        return Task.FromResult(book);
    }

    public Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        book.Id = _books.Max(b => b.Id) + 1;
        _books.Add(book);
        
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(book);
    }

    public Task<Book> UpdateAsync(int id, Book book, CancellationToken cancellationToken = default)
    {
        var index = _books.FindIndex(b => b.Id == id);
    
        if (index == -1)
            throw BookNotFoundException.ById(id);
    
        // Сохраняем оригинальный Id из параметра (на случай если во входящем объекте он другой)
        book.Id = id;
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
}
