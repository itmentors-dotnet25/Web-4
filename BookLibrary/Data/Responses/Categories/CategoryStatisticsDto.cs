using BookLibrary.Models;

namespace BookLibrary.Data.Responses.Categories;

public record CategoryStatisticsDto(
    string CategoryName,
    int BookCount,
    int AveragePublicationYear,
    List<BookSummaryDto>? Books
    );
