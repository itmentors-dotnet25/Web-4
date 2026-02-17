using BookLibrary.Data.Requests.Book;
using BookLibrary.Models;
using BookLibrary.Specifications.Book;

namespace BookLibrary.Contracts.Services;

public interface IBookService
{
    
    /// <summary>
    /// Получить все книги
    /// </summary>
    Task<IEnumerable<Book>> GetAllBooksAsync(BookFilterParams filterParams, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить книгу по идентификатору
    /// </summary>
    Task<Book> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Создать новую книгу
    /// </summary>
    Task<Book> CreateBookAsync(Book book, CancellationToken cancellationToken = default);
    
    // /// <summary>
    // /// Обновить книгу
    // /// </summary>
    Task<Book> UpdateBookAsync(int id, UpdateBookRequest data, CancellationToken cancellationToken = default);
    
    // /// <summary>
    // /// Удалить книгу
    // /// </summary>
    Task<bool> DeleteBookAsync(int id, CancellationToken cancellationToken = default);
}
