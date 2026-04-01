using BookLibrary.Models;

namespace BookLibrary.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Genre { get; set; }
    public bool IsAvailable { get; set; } = true;

    public int AuthorId { get; set; }
    public int CategoryId { get; set; }

    public Author Author { get; set; } = null!; 
    public Category Category { get; set; } = null!; 
}