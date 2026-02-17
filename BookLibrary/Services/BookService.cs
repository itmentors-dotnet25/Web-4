using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Data.Requests.Book;
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

    public async Task<Book> UpdateBookAsync(int id, UpdateBookRequest data, CancellationToken cancellationToken = default)
    {
        var book = new Book();
        data.ApplyTo(book);
        
        return await _repository.UpdateAsync(id, data, cancellationToken);
    }
    
    public async Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
