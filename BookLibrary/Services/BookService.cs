using System.Collections.ObjectModel;
using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Models;
using BookLibrary.Specifications.Book;

namespace BookLibrary.Services;

public class BookService(IBookRepository repository) : IBookService
{
    private readonly IBookRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<IEnumerable<Book>> GetAllBooksAsync(BookFilterParams filterParams, CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(filterParams, cancellationToken);
    }
    
    public async Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default)
    {
        return await _repository.CreateAsync(book, cancellationToken);
    }

    public async Task<Book> UpdateBookAsync(int id, Book book, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateAsync(id, book, cancellationToken);
    }
    
    public async Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
