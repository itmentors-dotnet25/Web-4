using BookLibrary.Contracts;
using BookLibrary.Models;

namespace BookLibrary.Mapping;

public static class BookMapping
{
    public static BookDto ToDto(this Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author is null
            ? string.Empty
            : $"{book.Author.FirstName} {book.Author.LastName}".Trim(),
        Category = book.Category?.Name ?? string.Empty,
        ISBN = book.ISBN,
        PublicationYear = book.PublicationYear,
        Genre = book.Genre,
        IsAvailable = book.IsAvailable
    };

    public static Book ToModel(this CreateBookRequest req) => new()
    {
        Title = req.Title,
        ISBN = req.ISBN,
        PublicationYear = req.PublicationYear,
        Genre = req.Genre,
        IsAvailable = req.IsAvailable,
        AuthorId = req.AuthorId,
        CategoryId = req.CategoryId
    };

    public static Book ToModel(this UpdateBookRequest req) => new()
    {
        Title = req.Title,
        ISBN = req.ISBN,
        PublicationYear = req.PublicationYear,
        Genre = req.Genre,
        IsAvailable = req.IsAvailable,
        AuthorId = req.AuthorId,
        CategoryId = req.CategoryId
    };
}