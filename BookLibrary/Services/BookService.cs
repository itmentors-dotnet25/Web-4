using BookLibrary.Contracts;
using BookLibrary.Models;
using BookLibrary.Repositories;

namespace BookLibrary.Services;

public class BookService(ILogger<BookService> logger, IBookRepository bookRepository) : IBookService
{
    public async Task<IEnumerable<Book>> GetAllBooksAsync(string? author = null, string? sortBy = null)
    {
        logger.LogInformation("Запрос на получение всех книг. author={Author}, sortBy={SortBy}", author, sortBy);
        return await bookRepository.GetAllAsync(author, sortBy);
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        logger.LogInformation("Запрос на получение книги с ID: {BookId}", id);
        return await bookRepository.GetByIdAsync(id);
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        logger.LogInformation("Запрос на создание книги. Title={Title}, ISBN={ISBN}", book.Title, book.ISBN);
        return await bookRepository.AddAsync(book);
    }

    public async Task<Book?> UpdateBookAsync(int id, Book book)
    {
        logger.LogInformation("Запрос на обновление книги с ID: {BookId}", id);
        return await bookRepository.UpdateAsync(id, book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        logger.LogInformation("Запрос на удаление книги с ID: {BookId}", id);
        return await bookRepository.RemoveAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooksWithDetailsAsync()
    {
        logger.LogInformation("Запрос на получение книг с детализацией.");
        return await bookRepository.GetBooksWithDetailsAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        logger.LogInformation("Запрос на получение книг автора с ID: {AuthorId}", authorId);
        return await bookRepository.GetBooksByAuthorIdAsync(authorId);
    }

    public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync()
    {
        logger.LogInformation("Запрос на получение статистики по категориям.");
        return await bookRepository.GetCategoryStatisticsAsync();
    }
}