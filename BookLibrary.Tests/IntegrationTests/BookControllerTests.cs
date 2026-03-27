using System.Net;
using System.Text;
using System.Text.Json;
using BookLibrary.Dto.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace BookLibrary.Tests.IntegrationTests;

public class BookControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;

    public BookControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient();
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        var response = await _client.GetAsync("api/v1/books");

        // _testOutputHelper.WriteLine($"Status: {response.StatusCode}");
        // _testOutputHelper.WriteLine($"Body: {await response.Content.ReadAsStringAsync()}");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookResponse>>>(json, _options);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetBookById_ReturnsBook()
    {
        var response = await _client.GetAsync("api/v1/books/1");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<BookResponse>>(json, _options);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var expected = new BookResponse(
            Id: 1,
            Title: "Clean Code",
            Author: "Robert C. Martin",
            Isbn: "978-11-1111-000-1",
            PublicationYear: 2008,
            Genre: "Technical literature",
            IsAvailable: true
        );

        result.Data.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetBookById_WithInvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("api/v1/books/9999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<object>>(json, _options);

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().NotBeNullOrWhiteSpace();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBooks_WithAuthorFilter_ReturnsFilteredBooks()
    {
        var response = await _client.GetAsync("api/v1/books?author=martin");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookResponse>>>(json, _options);

        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.Data.Should().AllSatisfy(b =>
            b.Author.ToLowerInvariant().Should().Contain("martin"));
    }

    [Fact]
    public async Task GetBooks_WithNonExistentAuthor_ReturnsEmptyList()
    {
        var response = await _client.GetAsync("api/v1/books?author=UnknownAuthor");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookResponse>>>(json, _options);

        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task GetBooks_WithSortByTitle_AppliesSorting()
    {
        var response = await _client.GetAsync("api/v1/books?sortBy=title");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookResponse>>>(json, _options);

        result.Should().NotBeNull();
        result.Data.Should().BeInAscendingOrder(x => x.Title);
    }

    [Fact]
    public async Task GetBooks_WithAuthorAndSortByTitle_AppliesBoth()
    {
        var response = await _client.GetAsync("api/v1/books?author=robert&sortBy=title");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<BookResponse>>>(json, _options);

        result.Should().NotBeNull();
        result.Data.Should().NotBeNullOrEmpty();
        result.Data.Should().AllSatisfy(b =>
            b.Author.ToLowerInvariant().Should().Contain("robert"));
        result.Data.Should().BeInAscendingOrder(x => x.Title);
    }

    [Fact]
    public async Task CreateBook_WithValidData_ReturnsCreated()
    {
        var newBook = new
        {
            title = "Test Book",
            author = "Test Author",
            isbn = "978-11-1111-111-1",
            publicationYear = 2026,
            genre = "Fiction",
            isAvailable = true
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newBook),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("api/v1/books", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<BookResponse>>(json);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data?.Title.Should().Be("Test Book");
    }

    [Fact]
    public async Task CreateBook_WithIncorrectData_ReturnsValidationError()
    {
        var invalidBook = new
        {
            author = "A",
            isbn = "123",
            publicationYear = 2026
        };

        var content = new StringContent(
            JsonSerializer.Serialize(invalidBook),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("api/v1/books", content);

        _testOutputHelper.WriteLine($"Status: {response.StatusCode}");
        _testOutputHelper.WriteLine($"Body: {await response.Content.ReadAsStringAsync()}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        string json = await response.Content.ReadAsStringAsync();
        var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(json, _options);

        problem.Should().NotBeNull();
        problem.Status.Should().Be(400);
        problem.Errors.Should().ContainKey("Title");
        problem.Errors.Should().ContainKey("Author");
        problem.Errors.Should().ContainKey("Isbn");
    }

    [Fact]
    public async Task UpdateBook_WithValidData_ReturnsUpdatedBook()
    {
        var updatedBook = new
        {
            id = 1,
            title = "Updated Clean Code",
            author = "Robert C. Martin",
            isbn = "978-11-1111-000-1",
            publicationYear = 2025,
            genre = "Refactored Literature",
            isAvailable = false
        };

        var content = new StringContent(
            JsonSerializer.Serialize(updatedBook),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PutAsync("api/v1/books/1", content);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<BookResponse>>(json, _options);

        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.Title.Should().Be("Updated Clean Code");
        result.Data.PublicationYear.Should().Be(2025);
        result.Data.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateBook_WithInvalidId_ReturnsNotFound()
    {
        var book = new
        {
            id = 999,
            title = "Non-existent",
            author = "Nobody",
            publicationYear = 2023
        };

        var content = new StringContent(
            JsonSerializer.Serialize(book),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PutAsync("api/v1/books/999", content);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteBook_ExistingId_ReturnsSuccess()
    {
        var response = await _client.DeleteAsync("api/v1/books/3");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<object>>(json, _options);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Message.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DeleteBook_NonExistentId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("api/v1/books/999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}