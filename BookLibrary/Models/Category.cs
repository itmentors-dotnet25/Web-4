using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BookLibrary.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Название категории
    /// </summary>
    [Required(ErrorMessage = "Название категории обязательно для заполнения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание категории
    /// </summary>
    [StringLength(1500, ErrorMessage = "Описание не должно превышать 1500 символов")]
    public string? Description { get; set; }

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

    /// <summary>
    /// Навигационное свойство: коллекция книг в этой категории
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
