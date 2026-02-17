using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Models;
using BookLibrary.Services;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.Services;

[Trait("Category", "Unit")]
public class AuthorServiceTests
{
    private readonly Mock<IAuthorReadRepository> _authorRepositoryMock;
    private readonly IAuthorService _authorService;

    public AuthorServiceTests()
    {
        _authorRepositoryMock = new Mock<IAuthorReadRepository>();
        _authorService = new AuthorService(_authorRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAuthorByIdAsync_ExistingId_ReturnsAuthor()
    {
        // Arrange
        var author = new Author 
        { 
            Id = 1, 
            Name = "Агата Кристи", 
            Country = "Великобритания",
            BirthYear = 1890,
            Biography = "Британская писательница",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _authorRepositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        // Act
        var result = await _authorService.GetAuthorByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Агата Кристи", result.Name);
        Assert.Equal("Великобритания", result.Country);
        Assert.True(result.IsActive);
    }
}
