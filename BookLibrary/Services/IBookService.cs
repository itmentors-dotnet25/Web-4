using BookLibrary.Models;

namespace BookLibrary.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync(string? author = null, string? sortBy = null);
    
    Task<Book?> GetBookByIdAsync(int id);
    
    Task<Book> CreateBookAsync(Book book);
    
    Task<Book?> UpdateBookAsync(int id, Book book);
    
    Task<bool> DeleteBookAsync(int id);
}