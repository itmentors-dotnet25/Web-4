using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Models;
using BookLibrary.Services;
using BookLibrary.Specifications.Book;
using Moq;
using Xunit.Abstractions;

namespace BookLibrary.Tests.UnitTests.Services;

[Trait("Category", "Unit")]
public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly IBookService _bookService;

    public BookServiceTests(ITestOutputHelper output)
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _bookService = new BookService(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllBooksAsync_ReturnsBooks()
    {
        // Arrange
        var books = new List<Book> { new() { Id = 1, Title = "Test Book" } };
        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<BookFilterParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        // Act
        var result = await _bookService.GetAllBooksAsync(new BookFilterParams());

        // Assert
        Assert.Single(result);
        Assert.Equal("Test Book", result.First().Title);
    }

    [Fact]
    public async Task GetBookByIdAsync_ReturnsBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book" };
        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        // Act
        var result = await _bookService.GetBookByIdAsync(1);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task CreateBookAsync_ReturnsCreatedBook()
    {
        // Arrange
        var newBook = new Book { Title = "New Book", Author = "Author" };
        var createdBook = new Book { Id = 100, Title = "New Book", Author = "Author" };
        
        _bookRepositoryMock
            .Setup(r => r.CreateAsync(newBook, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        // Act
        var result = await _bookService.CreateBookAsync(newBook);

        // Assert
        Assert.Equal(100, result.Id);
        Assert.Equal("New Book", result.Title);
    }

    [Fact]
    public async Task UpdateBookAsync_ReturnsUpdatedBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Updated Title" };
        _bookRepositoryMock
            .Setup(r => r.UpdateAsync(1, book, It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        // Act
        var result = await _bookService.UpdateBookAsync(1, book);

        // Assert
        Assert.Equal("Updated Title", result.Title);
    }

    [Fact]
    public async Task DeleteBookAsync_ReturnsTrue()
    {
        // Arrange
        _bookRepositoryMock
            .Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _bookService.DeleteBookAsync(1);

        // Assert
        Assert.True(result);
    }
}
