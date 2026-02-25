using BookLibrary.Models;
using BookLibrary.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookLibrary.Tests.Services;

public class BookServiceTests
{
    private readonly BookService _bookService;
    private readonly Mock<ILogger<BookService>> _loggerMock;

    public BookServiceTests()
    {
        _loggerMock = new Mock<ILogger<BookService>>();
        _bookService = new BookService(_loggerMock.Object);
    }

    [Fact]
    public async Task GetAllBooksAsync_ReturnsAllBooks()
    {
        var result = await _bookService.GetAllBooksAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllBooksAsync_WithAuthorFilter_ReturnsFilteredBooks()
    {
        var result = await _bookService.GetAllBooksAsync(author: "Tolkien");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.All(result, book => Assert.Contains("Tolkien", book.Author, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetAllBooksAsync_WithSortByTitle_ReturnsSortedBooks()
    {
        var result = await _bookService.GetAllBooksAsync(sortBy: "title");
        var booksList = result.ToList();

        Assert.NotNull(result);
        Assert.Equal(3, booksList.Count);

        Assert.True(booksList[0].Title.CompareTo(booksList[1].Title) <= 0);
        Assert.True(booksList[1].Title.CompareTo(booksList[2].Title) <= 0);
    }

    [Fact]
    public async Task GetBookByIdAsync_ExistingId_ReturnsBook()
    {
        var result = await _bookService.GetBookByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("The Lord of the Rings", result.Title);
        Assert.Equal("J.R.R. Tolkien", result.Author);
    }

    [Fact]
    public async Task GetBookByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _bookService.GetBookByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateBookAsync_ValidBook_CreatesBook()
    {
        var newBook = new Book
        {
            Title = "New Test Book",
            Author = "Test Author",
            ISBN = "123-45-6789-012-3",
            PublicationYear = 2024,
            Genre = "Test Genre",
            IsAvailable = true
        };

        var result = await _bookService.CreateBookAsync(newBook);

        Assert.NotNull(result);
        Assert.Equal("New Test Book", result.Title);
        Assert.Equal(4, result.Id);
    }

    [Fact]
    public async Task UpdateBookAsync_ExistingId_UpdatesBook()
    {
        var updatedBook = new Book
        {
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "123-45-6789-012-3",
            PublicationYear = 2024,
            Genre = "Updated Genre",
            IsAvailable = false
        };

        var result = await _bookService.UpdateBookAsync(1, updatedBook);

        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Author", result.Author);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdateBookAsync_NonExistingId_ReturnsNull()
    {
        var updatedBook = new Book
        {
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "123-45-6789-012-3",
            PublicationYear = 2024
        };

        var result = await _bookService.UpdateBookAsync(999, updatedBook);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteBookAsync_ExistingId_ReturnsTrue()
    {
        var result = await _bookService.DeleteBookAsync(1);

        Assert.True(result);

        var checkBook = await _bookService.GetBookByIdAsync(1);
        Assert.Null(checkBook);
    }

    [Fact]
    public async Task DeleteBookAsync_NonExistingId_ReturnsFalse()
    {
        var result = await _bookService.DeleteBookAsync(999);

        Assert.False(result);
    }
}