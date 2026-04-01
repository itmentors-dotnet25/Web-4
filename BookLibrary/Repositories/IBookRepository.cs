using BookLibrary.Models;
using BookLibrary.Contracts; 

namespace BookLibrary.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(string? author = null, string? sortBy = null);
    Task<Book?> GetByIdAsync(int id);
    Task<Book> AddAsync(Book book);
    Task<Book?> UpdateAsync(int id, Book book);
    Task<bool> RemoveAsync(int id);

    Task<IEnumerable<Book>> GetBooksWithDetailsAsync();
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);
    Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync();
}