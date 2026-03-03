namespace BookLibrary.Requests;

public class BookRequest
{
    /// <summary>
    /// Название книги
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Автор книги
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// ISBN-13 номер
    /// </summary>
    public string ISBN { get; set; } = string.Empty;

    /// <summary>
    /// Год публикации
    /// </summary>
    public int PublicationYear { get; set; }

    /// <summary>
    /// Жанр книги
    /// </summary>
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Доступна ли книга для выдачи
    /// </summary>
    public bool IsAvailable { get; set; }
}