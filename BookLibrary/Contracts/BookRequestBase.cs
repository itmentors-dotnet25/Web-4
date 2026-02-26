using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Contracts;

public abstract class BookRequestBase
{
    [Required(ErrorMessage = "Название книги обязательно для заполнения")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Название должно содержать от 1 до 200 символов")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Автор обязательно для заполнения")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Имя автора должно содержать от 1 до 200 символов")]
    public string Author { get; init; } = string.Empty;

    [Required(ErrorMessage = "ISBN обязательно для заполнения")]
    [RegularExpression(@"^\d{3}-\d{10}$", ErrorMessage = "ISBN должен быть в формате XXX-XXXXXXXXXX")]
    public string ISBN { get; init; } = string.Empty;

    [CurrentYearRange(1000, ErrorMessage = "Год издания должен быть между 1000 и текущим годом")]
    public int PublicationYear { get; init; }

    public string? Genre { get; init; }

    public bool IsAvailable { get; init; }
}