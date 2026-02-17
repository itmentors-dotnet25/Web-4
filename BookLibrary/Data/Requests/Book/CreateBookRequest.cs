using System.ComponentModel.DataAnnotations;
using BookLibrary.Models;

namespace BookLibrary.Data.Requests.Book;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Genre { get; set; }
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Преобразует запрос в модель книги
    /// </summary>
    public Models.Book ToBook() => new()
    {
        Title = Title,
        AuthorId = AuthorId,
        CategoryId = CategoryId,
        ISBN = ISBN,
        PublicationYear = PublicationYear,
        Genre = Genre,
        IsAvailable = IsAvailable
    };
}
