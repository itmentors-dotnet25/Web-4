using BookLibrary.Contracts;
using BookLibrary.Models;
using FluentValidation;

namespace BookLibrary.Services;

public class BookService(IBookRepository repository, IValidator<Book> validator) : IBookService
{
    public Task<List<Book>> GetAllAsync(string? author = null, string? sortBy = null)
    {
        var books = repository.GetAll().ToList();

        if (!string.IsNullOrWhiteSpace(author))
        {
            books = books
                .Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (string.Equals(sortBy, "title", StringComparison.OrdinalIgnoreCase))
        {
            books = books.OrderBy(b => b.Title).ToList();
        }

        return Task.FromResult(books);
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = repository.GetById(id);
        return Task.FromResult(book);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        await validator.ValidateAndThrowAsync(book);
        repository.Add(book);
        return book;
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        await validator.ValidateAndThrowAsync(book);
        return repository.Update(book);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(repository.Delete(id));
    }
}