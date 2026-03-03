// Models/Book.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Models;

public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Название книги
    /// </summary>
    [Required(ErrorMessage = "Название книги обязательно")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 200 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Автор книги
    /// </summary>
    [Required(ErrorMessage = "Автор обязателен")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Имя автора должно быть от 3 до 100 символов")]
    [Display(Name = "Автор")]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// ISBN-13 номер
    /// </summary>
    [Display(Name = "ISBN")]
    public string ISBN { get; set; } = string.Empty;

    /// <summary>
    /// Год публикации
    /// </summary>
    [Required(ErrorMessage = "Год издания обязателен")]
    [Range(1000, 9999, ErrorMessage = "Год должен быть в диапазоне от 1000 до 9999")]
    [Display(Name = "Год издания")]
    [DisplayFormat(DataFormatString = "{0:yyyy}")]
    public int PublicationYear { get; set; }

    /// <summary>
    /// Жанр книги
    /// </summary>
    [StringLength(50, ErrorMessage = "Жанр не должен превышать 50 символов")]
    [Display(Name = "Жанр")]
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Доступна ли книга для выдачи
    /// </summary>
    [Display(Name = "Активна")]
    public bool IsAvailable { get; set; }

}