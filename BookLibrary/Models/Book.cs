// Models/Book.cs
namespace BookLibrary.Models;

public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги
    /// </summary>{
    public int Id { get; set; }

    /// <summary>
    /// Название книги
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Автор книги
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// ISBN-13 номер
    /// </summary>
    public string ISBN { get; set; }

    /// <summary>
    /// Год публикации
    /// </summary>
    public int PublicationYear { get; set; }

    /// <summary>
    /// Жанр книги
    /// </summary>
    public string Genre { get; set; }

    /// <summary>
    /// Доступна ли книга для выдачи
    /// </summary>
    public bool IsAvailable { get; set; }
}