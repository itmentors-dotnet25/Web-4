using System.ComponentModel.DataAnnotations;
using BookLibrary.Validators;
using Newtonsoft.Json;

namespace BookLibrary.Models;

/// <summary>
/// Модель автора
/// </summary>
public class Author
{
    /// <summary>
    /// Уникальный идентификатор автора
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Имя автора
    /// </summary>
    [Required(ErrorMessage = "Имя автора обязательно для заполнения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 100 символов")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Страна автора
    /// </summary>
    [StringLength(50, ErrorMessage = "Страна не должна превышать 50 символов")]
    public string? Country { get; set; }

    /// <summary>
    /// Год рождения
    /// </summary>
    [YearRange(1000, ErrorMessage = "Год рождения должен быть в диапазоне от 1000 до текущего года")]
    public int? BirthYear { get; set; }

    /// <summary>
    /// Год смерти (если применимо)
    /// </summary>
    [YearRange(1000, allowFutureYears: false, ErrorMessage = "Год смерти должен быть в диапазоне от 1000 до текущего года")]
    public int? DeathYear { get; set; }

    /// <summary>
    /// Краткая биография
    /// </summary>
    [StringLength(1000, ErrorMessage = "Биография не должна превышать 1000 символов")]
    public string? Biography { get; set; }

    /// <summary>
    /// Доступность автора в каталоге
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Дата создания записи
    /// </summary>
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    [JsonIgnore]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public IEnumerable<Book>? Books { get; set; }
}
