using BookLibrary.Data.Requests.Book;
using BookLibrary.Data.Responses.Categories;
using BookLibrary.Models;
using BookLibrary.Specifications.Book;

namespace BookLibrary.Contracts.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync(BookFilterParams? filterParams = null, CancellationToken cancellationToken = default);
    Task<Book> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Book> CreateAsync(Book book, CancellationToken cancellationToken = default);
    Task<Book> UpdateAsync(int id, UpdateBookRequest data, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<CategoryStatisticsDto>> GetCategoryStatisticsAsync(
        bool withBooks = false,
        CancellationToken cancellationToken = default);
}
