using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Название книги обязательно для заполнения")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Название должно содержать от 1 до 200 символов")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Автор обязательно для заполнения")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Имя автора должно содержать от 1 до 200 символов")]
    public string Author { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "ISBN обязательно для заполнения")]
    [RegularExpression(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d{1}$", 
        ErrorMessage = "ISBN должен быть в формате XXX-XX-XXXX-XXX-X")]
    public string ISBN { get; set; } = string.Empty;
    
    [Range(1000, 2026, ErrorMessage = "Год издания должен быть между 1000 и текущим годом")]
    public int PublicationYear { get; set; }
    
    public string? Genre { get; set; }
    
    public bool IsAvailable { get; set; } = true;
}