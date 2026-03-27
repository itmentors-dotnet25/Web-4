namespace BookLibrary.Dto.Responses;

public record BookResponse(
    int Id,
    string Title,
    string Author,
    string? Isbn,
    int PublicationYear,
    string? Genre,
    bool IsAvailable
);