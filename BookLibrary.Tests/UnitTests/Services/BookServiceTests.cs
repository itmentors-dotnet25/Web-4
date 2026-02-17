using BookLibrary.Contracts.Repositories;
using BookLibrary.Contracts.Services;
using BookLibrary.Data.Requests.Book;
using BookLibrary.Models;
using BookLibrary.Services;
using BookLibrary.Specifications.Book;
using Moq;
using Xunit;

namespace BookLibrary.Tests.UnitTests.Services;

[Trait("Category", "Unit")]
public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly IBookService _bookService;

    public BookServiceTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _bookService = new BookService(_bookRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllBooksAsync_WithFilterParams_CallsRepositoryCorrectly()
    {
        // Arrange
        var filterParams = new BookFilterParams 
        { 
            SortBy = "title", 
            
            WithAuthors = true,
            WithCategories = true
        };
        var books = new List<Book> { new() { Id = 1, Title = "Test Book", AuthorId = 1, CategoryId = 1 } };
        
        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(It.Is<BookFilterParams>(fp => 
                fp.SortBy == "title" && 
                fp.WithAuthors == true &&
                fp.WithCategories == true), It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        // Act
        var result = await _bookService.GetAllBooksAsync(filterParams);

        // Assert
        Assert.Single(result);
        _bookRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<BookFilterParams>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBookByIdAsync_ExistingId_ReturnsBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book", AuthorId = 1, CategoryId = 1 };
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
    public async Task CreateBookAsync_ValidRequest_ReturnsCreatedBook()
    {
        // Arrange
        var request = new Book
        {
            Title = "New Book",
            AuthorId = 1,
            CategoryId = 1,
            ISBN = "978-3-16-148410-0",
            PublicationYear = 2024,
            Genre = "Fiction",
            IsAvailable = true
        };
        var createdBook = new Book 
        { 
            Id = 100, 
            Title = "New Book", 
            AuthorId = 1, 
            CategoryId = 1,
            ISBN = "978-3-16-148410-0",
            PublicationYear = 2024,
            Genre = "Fiction",
            IsAvailable = true
        };
        
        _bookRepositoryMock
            .Setup(r => r.CreateAsync(It.Is<Book>(b => 
                b.Title == "New Book" && 
                b.AuthorId == 1), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        // Act
        var result = await _bookService.CreateBookAsync(request);

        // Assert
        Assert.Equal(100, result.Id);
        Assert.Equal("New Book", result.Title);
    }

    [Fact]
    public async Task UpdateBookAsync_ValidRequest_UpdatesBook()
    {
        // Arrange
        var request = new UpdateBookRequest
        {
            Title = "Updated Title",
            AuthorId = 1,
            CategoryId = 1,
            ISBN = "978-3-16-148410-0",
            PublicationYear = 2025,
            Genre = "Non-Fiction",
            IsAvailable = false
        };
        var updatedBook = new Book 
        { 
            Id = 1, 
            Title = "Updated Title", 
            AuthorId = 1, 
            CategoryId = 1,
            ISBN = "978-3-16-148410-0",
            PublicationYear = 2025,
            Genre = "Non-Fiction",
            IsAvailable = false
        };
        
        _bookRepositoryMock
            .Setup(r => r.UpdateAsync(1, request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedBook);

        // Act
        var result = await _bookService.UpdateBookAsync(1, request);

        // Assert
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Non-Fiction", result.Genre);
    }

    [Fact]
    public async Task DeleteBookAsync_ExistingId_ReturnsTrue()
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
