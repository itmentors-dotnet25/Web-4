using BookLibrary.Contracts;
using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories;

public class EfBookRepository(ApplicationDbContext context, ILogger<EfBookRepository> logger) : IBookRepository
{
    public async Task<IEnumerable<Book>> GetAllAsync(string? author = null, string? sortBy = null)
    {
        IQueryable<Book> query = context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category);

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b =>
                (b.Author.FirstName + " " + b.Author.LastName).Contains(author));
        }

        if (string.Equals(sortBy, "title", StringComparison.OrdinalIgnoreCase))
        {
            query = query.OrderBy(b => b.Title);
        }

        return await query.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        var isbnExists = await context.Books.AnyAsync(b => b.ISBN == book.ISBN);
        if (isbnExists)
        {
            throw new InvalidOperationException($"Книга с ISBN '{book.ISBN}' уже существует.");
        }

        context.Books.Add(book);
        await context.SaveChangesAsync();

        return await context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstAsync(b => b.Id == book.Id);
    }

    public async Task<Book?> UpdateAsync(int id, Book book)
    {
        var existingBook = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (existingBook is null)
        {
            return null;
        }

        var duplicateIsbnExists = await context.Books.AnyAsync(b => b.Id != id && b.ISBN == book.ISBN);
        if (duplicateIsbnExists)
        {
            throw new InvalidOperationException($"Книга с ISBN '{book.ISBN}' уже существует.");
        }

        existingBook.Title = book.Title;
        existingBook.AuthorId = book.AuthorId;
        existingBook.CategoryId = book.CategoryId;
        existingBook.ISBN = book.ISBN;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.Genre = book.Genre;
        existingBook.IsAvailable = book.IsAvailable;

        await context.SaveChangesAsync();

        return await context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstAsync(b => b.Id == id);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book is null)
        {
            return false;
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Book>> GetBooksWithDetailsAsync()
    {
        logger.LogInformation("Запрос на получение книг с детализацией.");

        return await context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
    {
        logger.LogInformation("Запрос на получение книг автора с ID: {AuthorId}", authorId);

        return await context.Books
            .AsNoTracking()
            .Where(b => b.AuthorId == authorId)
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatisticsAsync()
    {
        logger.LogInformation("Запрос на получение статистики по категориям.");

        return await context.Categories
            .AsNoTracking()
            .Select(c => new CategoryStatsDto
            {
                Name = c.Name,
                Count = c.Books.Count,
                AveragePublicationYear = c.Books.Any()
                    ? c.Books.Average(b => b.PublicationYear)
                    : 0
            })
            .ToListAsync();
    }
}