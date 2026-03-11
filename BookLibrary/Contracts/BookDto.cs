namespace BookLibrary.Contracts;

public sealed class BookDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string ISBN { get; init; } = string.Empty;
    public int PublicationYear { get; init; }
    public string? Genre { get; init; }
    public bool IsAvailable { get; init; }
}