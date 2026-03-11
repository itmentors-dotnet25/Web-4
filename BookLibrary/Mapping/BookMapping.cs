using BookLibrary.Contracts;
using BookLibrary.Models;

namespace BookLibrary.Mapping;

public static class BookMapping
{
    public static BookDto ToDto(this Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        ISBN = book.ISBN,
        PublicationYear = book.PublicationYear,
        Genre = book.Genre,
        IsAvailable = book.IsAvailable
    };

    public static Book ToModel(this CreateBookRequest req) => new()
    {
        Title = req.Title,
        Author = req.Author,
        ISBN = req.ISBN,
        PublicationYear = req.PublicationYear,
        Genre = req.Genre,
        IsAvailable = req.IsAvailable
    };

    public static Book ToModel(this UpdateBookRequest req) => new()
    {
        Title = req.Title,
        Author = req.Author,
        ISBN = req.ISBN,
        PublicationYear = req.PublicationYear,
        Genre = req.Genre,
        IsAvailable = req.IsAvailable
    };
}