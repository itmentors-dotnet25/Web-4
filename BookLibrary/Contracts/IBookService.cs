using BookLibrary.Models;

namespace BookLibrary.Contracts;

public interface IBookService
{
    Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null);
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<bool> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
}