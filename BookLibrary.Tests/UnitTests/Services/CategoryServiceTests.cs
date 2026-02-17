using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Data.Responses.Categories;
using BookLibrary.Services;
using Moq;

namespace BookLibrary.Tests.UnitTests.Services;

[Trait("Category", "Unit")]
public class CategoryServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly ICategoryService _categoryService;

    public CategoryServiceTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _categoryService = new CategoryService(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task GetStatisticsAsync_WithoutBooks_ReturnsStatisticsWithoutBooks()
    {
        // Arrange
        var stats = new List<CategoryStatisticsDto>
        {
            new("Детектив", 3, 1950, null),
            new("Фэнтези", 1, 1997, null)
        };
        
        _bookRepositoryMock
            .Setup(r => r.GetCategoryStatisticsAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stats);

        // Act
        var result = (await _categoryService.GetStatisticsAsync(withBooks: false)).ToList();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, s => Assert.Null(s.Books));
        Assert.Equal("Детектив", result.First().CategoryName);
        Assert.Equal(3, result.First().BookCount);
    }

    [Fact]
    public async Task GetStatisticsAsync_WithBooks_ReturnsStatisticsWithBooks()
    {
        // Arrange
        var stats = new List<CategoryStatisticsDto>
        {
            new("Детектив", 3, 1950, new List<BookSummaryDto>
            {
                new(1, "Десять негритят", 1939),
                new(2, "Убийство в Восточном экспрессе", 1934)
            })
        };
        
        _bookRepositoryMock
            .Setup(r => r.GetCategoryStatisticsAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stats);

        // Act
        var result = (await _categoryService.GetStatisticsAsync(withBooks: true)).ToList();
        var first = result.First();

        // Assert
        Assert.Single(result);
        Assert.NotNull(first.Books);
        Assert.Equal(2, first.Books.Count);
        Assert.Equal("Десять негритят", first.Books[0].Title);
        Assert.Equal(1939, first.Books[0].PublicationYear);
    }
}
