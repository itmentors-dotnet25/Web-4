using BookLibrary.Contracts;
using BookLibrary.Models;
using BookLibrary.Repositories;
using BookLibrary.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookLibrary.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<ILogger<BookService>> _loggerMock = new();
    private readonly BookService _service;

    public BookServiceTests()
    {
        _service = new BookService(_loggerMock.Object, _bookRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllBooksAsync_ReturnsBooks()
    {
        var books = new List<Book>
        {
            CreateBook(1, "1984", "978-12-3456-789-0"),
            CreateBook(2, "Хоббит", "978-12-3456-789-1")
        };

        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(null, null))
            .ReturnsAsync(books);

        var result = await _service.GetAllBooksAsync();

        var resultList = result.ToList();

        Assert.Equal(2, resultList.Count);
        Assert.Equal("1984", resultList[0].Title);
        Assert.Equal("Хоббит", resultList[1].Title);
    }

    [Fact]
    public async Task GetAllBooksAsync_WithAuthorFilter_PassesArgumentsToRepository()
    {
        const string author = "Толстой";
        var books = new List<Book> { CreateBook(1, "Война и мир", "978-12-3456-789-2") };

        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(author, null))
            .ReturnsAsync(books);

        var result = await _service.GetAllBooksAsync(author);

        Assert.Single(result);
        _bookRepositoryMock.Verify(r => r.GetAllAsync(author, null), Times.Once);
    }

    [Fact]
    public async Task GetAllBooksAsync_WithSortByTitle_PassesArgumentsToRepository()
    {
        const string sortBy = "title";
        var books = new List<Book>
        {
            CreateBook(1, "Азбука", "978-12-3456-789-3"),
            CreateBook(2, "Букварь", "978-12-3456-789-4")
        };

        _bookRepositoryMock
            .Setup(r => r.GetAllAsync(null, sortBy))
            .ReturnsAsync(books);

        var result = await _service.GetAllBooksAsync(sortBy: sortBy);

        Assert.Equal(2, result.Count());
        _bookRepositoryMock.Verify(r => r.GetAllAsync(null, sortBy), Times.Once);
    }

    [Fact]
    public async Task GetBookByIdAsync_WhenBookExists_ReturnsBook()
    {
        var book = CreateBook(10, "Чистый код", "978-12-3456-789-5");

        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(book);

        var result = await _service.GetBookByIdAsync(10);

        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("Чистый код", result.Title);
    }

    [Fact]
    public async Task GetBookByIdAsync_WhenBookDoesNotExist_ReturnsNull()
    {
        _bookRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Book?)null);

        var result = await _service.GetBookByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateBookAsync_ReturnsCreatedBook()
    {
        var bookToCreate = CreateBook(0, "Предметно-ориентированное проектирование", "978-12-3456-789-6");
        var createdBook = CreateBook(5, "Предметно-ориентированное проектирование", "978-12-3456-789-6");

        _bookRepositoryMock
            .Setup(r => r.AddAsync(bookToCreate))
            .ReturnsAsync(createdBook);

        var result = await _service.CreateBookAsync(bookToCreate);

        Assert.Equal(5, result.Id);
        Assert.Equal("Предметно-ориентированное проектирование", result.Title);
        _bookRepositoryMock.Verify(r => r.AddAsync(bookToCreate), Times.Once);
    }

    [Fact]
    public async Task CreateBookAsync_WhenRepositoryThrows_RethrowsException()
    {
        var bookToCreate = CreateBook(0, "Дубликат ISBN", "978-12-3456-789-7");

        _bookRepositoryMock
            .Setup(r => r.AddAsync(bookToCreate))
            .ThrowsAsync(new InvalidOperationException("Книга с таким ISBN уже существует."));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateBookAsync(bookToCreate));
    }

    [Fact]
    public async Task UpdateBookAsync_WhenBookExists_ReturnsUpdatedBook()
    {
        var updatedBook = CreateBook(3, "Рефакторинг", "978-12-3456-789-8");

        _bookRepositoryMock
            .Setup(r => r.UpdateAsync(3, updatedBook))
            .ReturnsAsync(updatedBook);

        var result = await _service.UpdateBookAsync(3, updatedBook);

        Assert.NotNull(result);
        Assert.Equal(3, result!.Id);
        Assert.Equal("Рефакторинг", result.Title);
    }

    [Fact]
    public async Task UpdateBookAsync_WhenBookDoesNotExist_ReturnsNull()
    {
        var updatedBook = CreateBook(999, "Отсутствующая книга", "978-12-3456-789-9");

        _bookRepositoryMock
            .Setup(r => r.UpdateAsync(999, updatedBook))
            .ReturnsAsync((Book?)null);

        var result = await _service.UpdateBookAsync(999, updatedBook);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteBookAsync_WhenBookExists_ReturnsTrue()
    {
        _bookRepositoryMock
            .Setup(r => r.RemoveAsync(1))
            .ReturnsAsync(true);

        var result = await _service.DeleteBookAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteBookAsync_WhenBookDoesNotExist_ReturnsFalse()
    {
        _bookRepositoryMock
            .Setup(r => r.RemoveAsync(404))
            .ReturnsAsync(false);

        var result = await _service.DeleteBookAsync(404);

        Assert.False(result);
    }

    [Fact]
    public async Task GetBooksWithDetailsAsync_ReturnsBooks()
    {
        var books = new List<Book>
        {
            CreateBook(1, "Книга 1", "978-12-3456-780-0"),
            CreateBook(2, "Книга 2", "978-12-3456-780-1")
        };

        _bookRepositoryMock
            .Setup(r => r.GetBooksWithDetailsAsync())
            .ReturnsAsync(books);

        var result = await _service.GetBooksWithDetailsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetBooksByAuthorIdAsync_ReturnsBooks()
    {
        var books = new List<Book>
        {
            CreateBook(1, "Книга А", "978-12-3456-780-2", authorId: 7),
            CreateBook(2, "Книга Б", "978-12-3456-780-3", authorId: 7)
        };

        _bookRepositoryMock
            .Setup(r => r.GetBooksByAuthorIdAsync(7))
            .ReturnsAsync(books);

        var result = await _service.GetBooksByAuthorIdAsync(7);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetCategoryStatisticsAsync_ReturnsStats()
    {
        var stats = new List<CategoryStatsDto>
        {
            new() { Id = 1, Name = "Проза", Count = 2 },
            new() { Id = 2, Name = "Поэзия", Count = 1 }
        };

        _bookRepositoryMock
            .Setup(r => r.GetCategoryStatisticsAsync())
            .ReturnsAsync(stats);

        var result = await _service.GetCategoryStatisticsAsync();

        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("Проза", resultList[0].Name);
        Assert.Equal(2, resultList[0].Count);
    }

    private static Book CreateBook(
        int id,
        string title,
        string isbn,
        int authorId = 1,
        int categoryId = 1)
    {
        return new Book
        {
            Id = id,
            Title = title,
            ISBN = isbn,
            PublicationYear = 2020,
            Genre = "Тестовый жанр",
            IsAvailable = true,
            AuthorId = authorId,
            CategoryId = categoryId,
            Author = new Author
            {
                Id = authorId,
                FirstName = "Тест",
                LastName = "Автор"
            },
            Category = new Category
            {
                Id = categoryId,
                Name = "Тестовая категория"
            }
        };
    }
}