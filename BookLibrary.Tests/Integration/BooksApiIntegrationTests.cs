using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BookLibrary.Contracts;
using Xunit;

namespace BookLibrary.Tests.Integration;

public class BooksApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BooksApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        factory.ResetDatabase();
    }

    [Fact]
    public async Task GetAllBooks_ReturnsOkAndWrappedResponse()
    {
        var response = await _client.GetAsync("/api/books");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<List<BookDto>>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.NotNull(payload!.Data);
        Assert.Equal(2, payload.Data!.Count);
        Assert.Contains(payload.Data, b => b.Title == "Евгений Онегин");
        Assert.Contains(payload.Data, b => b.Title == "Война и мир");
    }

    [Fact]
    public async Task GetBookById_WhenBookExists_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/books/1");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<BookDto>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.True(payload!.Success);
        Assert.NotNull(payload.Data);
        Assert.Equal(1, payload.Data!.Id);
        Assert.Equal("Евгений Онегин", payload.Data.Title);
    }

    [Fact]
    public async Task GetBookById_WhenBookDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/books/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<BookDto>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.False(payload!.Success);
        Assert.Null(payload.Data);
    }

    [Fact]
    public async Task GetAllBooks_WithAuthorFilter_ReturnsFilteredBooks()
    {
        var response = await _client.GetAsync("/api/books?author=Толстой");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<List<BookDto>>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.True(payload!.Success);
        Assert.NotNull(payload.Data);
        Assert.Single(payload.Data!);
        Assert.Contains("Толстой", payload.Data[0].Author, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetAllBooks_WithSortByTitle_ReturnsSortedBooks()
    {
        var response = await _client.GetAsync("/api/books?sortBy=title");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<List<BookDto>>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.True(payload!.Success);
        Assert.NotNull(payload.Data);

        var titles = payload.Data!.Select(b => b.Title).ToList();
        var sortedTitles = titles.OrderBy(t => t).ToList();

        Assert.Equal(sortedTitles, titles);
    }

    [Fact]
    public async Task CreateBook_WithValidRequest_ReturnsCreated()
    {
        var request = new CreateBookRequest
        {
            Title = "Новая книга",
            ISBN = "999-99-9999-999-9",
            PublicationYear = 2021,
            Genre = "Техническая литература",
            IsAvailable = true,
            AuthorId = 1,
            CategoryId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/books", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<BookDto>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.True(payload!.Success);
        Assert.NotNull(payload.Data);
        Assert.Equal("Новая книга", payload.Data!.Title);
        Assert.Equal("999-99-9999-999-9", payload.Data.ISBN);
    }

    [Fact]
    public async Task CreateBook_WithInvalidRequest_ReturnsBadRequest()
    {
        var request = new CreateBookRequest
        {
            Title = "",
            ISBN = "invalid-isbn",
            PublicationYear = 999,
            AuthorId = 0,
            CategoryId = 0
        };

        var response = await _client.PostAsJsonAsync("/api/books", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBook_WithValidRequest_ReturnsOk()
    {
        var request = new UpdateBookRequest
        {
            Title = "Обновлённая книга",
            ISBN = "978-50-4123-457-4",
            PublicationYear = 1869,
            Genre = "Роман",
            IsAvailable = false,
            AuthorId = 2,
            CategoryId = 2
        };

        var response = await _client.PutAsJsonAsync("/api/books/2", request);

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<BookDto>>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.True(payload!.Success);
        Assert.NotNull(payload.Data);
        Assert.Equal("Обновлённая книга", payload.Data!.Title);
        Assert.False(payload.Data.IsAvailable);
    }

    [Fact]
    public async Task UpdateBook_WhenBookDoesNotExist_ReturnsNotFound()
    {
        var request = new UpdateBookRequest
        {
            Title = "Отсутствующая книга",
            ISBN = "888-88-8888-888-8",
            PublicationYear = 2020,
            Genre = "Тест",
            IsAvailable = true,
            AuthorId = 1,
            CategoryId = 1
        };

        var response = await _client.PutAsJsonAsync("/api/books/999", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_WhenBookExists_ReturnsOk()
    {
        var response = await _client.DeleteAsync("/api/books/2");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.True(payload!.Success);
    }

    [Fact]
    public async Task DeleteBook_WhenBookDoesNotExist_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/api/books/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse>(_jsonOptions);

        Assert.NotNull(payload);
        Assert.False(payload!.Success);
    }

    [Fact]
    public async Task NonExistentRoute_ReturnsStandardizedJson()
    {
        var response = await _client.GetAsync("/api/nonexistent");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;

        Assert.False(root.GetProperty("success").GetBoolean());
        Assert.Equal("Route not found", root.GetProperty("message").GetString());
        Assert.Equal("/api/nonexistent", root.GetProperty("path").GetString());
        Assert.True(root.TryGetProperty("timestamp", out _));
    }
}