using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BookLibrary.Models;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    [ForeignKey(nameof(Author))] 
    public int AuthorId { get; set; }
    
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    
    public string ISBN { get; set; } = string.Empty;
    
    public int PublicationYear { get; set; }
    
    public string? Genre { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [JsonIgnore]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Навигационное свойство: автор книги
    /// </summary>
    [ForeignKey(nameof(AuthorId))]
    public virtual Author? Author { get; set; }

    /// <summary>
    /// Навигационное свойство: категория книги
    /// </summary>
    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }
}
