using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models;

public class Book
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Название книги обязательно для заполнения")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Название должно быть от 1 до 255 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Автор книги обязателен для заполнения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Автор должен быть от 2 до 100 символов")]
    [Display(Name = "Автор")]
    public string Author { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "ISBN обязателен для заполнения")]
    [StringLength(17, MinimumLength = 10, ErrorMessage = "'{0}' должно быть длиной {2} символа(ов). Количество введенных символов: {1}.")]
    [RegularExpression(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d$", ErrorMessage = "ISBN должен быть в формате XXX-XX-XXXX-XXX-X (например: 978-56-9912-345-6)")]
    [Display(Name = "ISBN")]
    public string ISBN { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Год публикации обязателен для заполнения")]
    [Range(1000, 2026, ErrorMessage = "Год публикации должен быть в диапазоне от 1000 до 2026")]
    [Display(Name = "Год публикации")]
    public int PublicationYear { get; set; }
    
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Жанр должен быть от 1 до 50 символов")]
    [Display(Name = "Жанр")]
    public string? Genre { get; set; }
    
    [Display(Name = "Доступна")]
    public bool IsAvailable { get; set; } = true;
    
    [Display(Name = "Дата создания")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Display(Name = "Дата обновления")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
