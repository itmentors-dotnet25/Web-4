using System.Net;
using System.Net.Http.Json;
using BookLibrary.Data.Responses;
using BookLibrary.Models;
using Xunit;

namespace BookLibrary.Tests.IntegrationTests;

[Trait("Category", "Integration")]
public class BooksControllerTests(BookLibraryWebApplicationFactory factory) : IClassFixture<BookLibraryWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetBooks_ReturnsListOfBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/books");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal("Operation completed successfully", apiResponse.Message);
    }

    [Fact]
    public async Task GetBookById_ExistingId_ReturnsBook()
    {
        // Arrange
        var listResponse = await _client.GetAsync("/api/books");
        listResponse.EnsureSuccessStatusCode();
        
        var listApiResponse = await listResponse.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(listApiResponse?.Data);
        Assert.NotEmpty(listApiResponse.Data);
        
        var existingBookId = listApiResponse.Data.First().Id;
        
        // Act
        var response = await _client.GetAsync($"/api/books/{existingBookId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(existingBookId, apiResponse.Data.Id);
    }

    [Fact]
    public async Task GetBookById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var nonExistingBookId = 99999;
        
        // Act
        var response = await _client.GetAsync($"/api/books/{nonExistingBookId}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateBook_ValidBook_ReturnsCreatedBook()
    {
        // Arrange - обязательно добавляем обязательные поля для прохождения валидации
        var now = DateTime.UtcNow;
        var newBook = new Book
        {
            Title = "Test Book Integration",
            Author = "Test Author",
            ISBN = "123-45-6789-012-3",
            PublicationYear = 2024,
            Genre = "Fiction",
            IsAvailable = true,
            CreatedAt = now,
            UpdatedAt = now
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/books", newBook);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal("Resource created successfully", apiResponse.Message);
        Assert.Equal("Test Book Integration", apiResponse.Data.Title);
    }

    [Fact]
    public async Task CreateBook_InvalidData_ReturnsUnprocessableEntity()
    {
        // Arrange - пустое название вызовет ошибку валидации
        var invalidBook = new Book
        {
            Title = "",
            Author = "Author",
            ISBN = "123",
            PublicationYear = 100,
            Genre = "Fiction",
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/books", invalidBook);
        
        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBook_ExistingId_ReturnsUpdatedBook()
    {
        // Arrange
        var listResponse = await _client.GetAsync("/api/books");
        listResponse.EnsureSuccessStatusCode();
        
        var listApiResponse = await listResponse.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(listApiResponse?.Data);
        Assert.NotEmpty(listApiResponse.Data);
        
        var existingBook = listApiResponse.Data.First();
        
        // Обязательно добавляем временные метки
        var now = DateTime.UtcNow;
        var updatedBook = new Book
        {
            Id = existingBook.Id,
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "098-76-5432-109-8",
            PublicationYear = 2025,
            Genre = "Non-Fiction",
            IsAvailable = false,
            CreatedAt = existingBook.CreatedAt,
            UpdatedAt = now
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/books/{existingBook.Id}", updatedBook);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal("Updated Title", apiResponse.Data.Title);
    }

    [Fact]
    public async Task DeleteBook_ExistingId_ReturnsSuccess()
    {
        // Arrange - создаем книгу с обязательными полями
        var now = DateTime.UtcNow;
        var newBook = new Book
        {
            Title = "Book to Delete",
            Author = "Author",
            ISBN = "123-45-6789-012-3",
            PublicationYear = 2024,
            Genre = "Fiction",
            IsAvailable = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        var createResponse = await _client.PostAsJsonAsync("/api/books", newBook);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        
        var createApiResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.NotNull(createApiResponse?.Data);
        var bookId = createApiResponse.Data.Id;
        
        // Act
        var response = await _client.DeleteAsync($"/api/books/{bookId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var deleteApiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<object>>();
        Assert.NotNull(deleteApiResponse);
        Assert.True(deleteApiResponse.Success);
        
        // Проверяем, что книга удалена
        var getResponse = await _client.GetAsync($"/api/books/{bookId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_NonExistingId_ReturnsBadRequestWithFalseResult()
    {
        // Arrange
        var nonExistingBookId = 99999;
        
        // Act
        var response = await _client.DeleteAsync($"/api/books/{nonExistingBookId}");
        
        // Assert - текущее поведение системы: возвращает BadRequest с { result: false }
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<object>>();
        Assert.NotNull(apiResponse);
        Assert.False(apiResponse.Success);
    }
}
