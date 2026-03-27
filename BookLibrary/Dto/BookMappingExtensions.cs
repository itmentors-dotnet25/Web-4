using BookLibrary.Dto.Requests;
using BookLibrary.Dto.Responses;
using BookLibrary.Models;

namespace BookLibrary.Dto;

public static class BookMappingExtensions
{
    public static BookResponse ToResponse(this Book book)
    {
        return new BookResponse(
            Id: book.Id,
            Title: book.Title,
            Author: book.Author,
            Isbn: book.Isbn,
            PublicationYear: book.PublicationYear,
            Genre: book.Genre,
            IsAvailable: book.IsAvailable
        );
    }

    public static List<BookResponse> ToResponseList(this IEnumerable<Book> books) =>
        books.Select(b => b.ToResponse()).ToList();

    public static Book ToBook(this BookRequest request, int id = 0) =>
        new()
        {
            Id = id,
            Title = request.Title,
            Author = request.Author,
            Isbn = request.Isbn,
            PublicationYear = request.PublicationYear,
            Genre = request.Genre,
            IsAvailable = request.IsAvailable
        };
}