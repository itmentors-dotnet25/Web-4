using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Data.Requests.Book;

public class UpdateBookRequest
{
    /// <summary>
    /// Идентификатор книги для обновления
    /// </summary>
    [Required]
    public int Id { get; set; }
    
    /// <summary>
    /// Новое название книги (опционально)
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Новый автор (опционально)
    /// </summary>
    public int? AuthorId { get; set; }
    
    /// <summary>
    /// Новая категория (опционально)
    /// </summary>
    public int? CategoryId { get; set; }
    
    /// <summary>
    /// Новый ISBN (опционально)
    /// </summary>
    public string? ISBN { get; set; }
    
    /// <summary>
    /// Новый год публикации (опционально)
    /// </summary>
    public int? PublicationYear { get; set; }
    
    /// <summary>
    /// Новый жанр (опционально)
    /// </summary>
    public string? Genre { get; set; }
    
    /// <summary>
    /// Новый статус доступности (опционально)
    /// </summary>
    public bool? IsAvailable { get; set; }

    /// <summary>
    /// Применяет изменения к существующей модели книги
    /// </summary>
    public void ApplyTo(Models.Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (Title != null)
            book.Title = Title;
        
        if (AuthorId.HasValue)
            book.AuthorId = AuthorId.Value;
        
        if (CategoryId.HasValue)
            book.CategoryId = CategoryId.Value;
        
        if (ISBN != null)
            book.ISBN = ISBN;
        
        if (PublicationYear.HasValue)
            book.PublicationYear = PublicationYear.Value;
        
        if (Genre != null)
            book.Genre = Genre;
        
        if (IsAvailable.HasValue)
            book.IsAvailable = IsAvailable.Value;
    }
}
