using BookLibrary.Contracts;
using BookLibrary.Models;

namespace BookLibrary.Mapping;

public static class BookMapping
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = $"{book.Author.FirstName} {book.Author.LastName}".Trim(),
            Category = book.Category.Name,
            ISBN = book.ISBN,
            PublicationYear = book.PublicationYear,
            Genre = book.Genre,
            IsAvailable = book.IsAvailable
        };
    }

    public static Book ToModel(this CreateBookRequest request)
    {
        return new Book
        {
            Title = request.Title,
            ISBN = request.ISBN,
            PublicationYear = request.PublicationYear,
            Genre = request.Genre,
            IsAvailable = request.IsAvailable,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId
        };
    }

    public static Book ToModel(this UpdateBookRequest request)
    {
        return new Book
        {
            Title = request.Title,
            ISBN = request.ISBN,
            PublicationYear = request.PublicationYear,
            Genre = request.Genre,
            IsAvailable = request.IsAvailable,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId
        };
    }
}