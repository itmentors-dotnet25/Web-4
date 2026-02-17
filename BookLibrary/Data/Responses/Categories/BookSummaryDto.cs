namespace BookLibrary.Data.Responses.Categories;

public record BookSummaryDto(
    int Id,
    string Title,
    int PublicationYear
);

