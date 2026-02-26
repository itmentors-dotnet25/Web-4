using BookLibrary.Models;
using BookLibrary.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using BookLibrary.Contracts;

namespace BookLibrary.Tests.Controllers;

public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient()
    {
        BookService.ResetForTests();

        var f = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IBookService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton<IBookService>(sp =>
                    new BookService(sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<BookService>>())
                );
            });
        });

        return f.CreateClient();
    }

    [Fact]
    public async Task GetAllBooks_ReturnsOk_AndNonEmptyList()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Book>>>();

        Assert.NotNull(payload);
        Assert.True(payload.Success);
        Assert.NotNull(payload.Data);
        Assert.NotEmpty(payload.Data!);
    }

    [Fact]
    public async Task GetBookById_NotFound_Returns404()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/books/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<Book>>();

        Assert.NotNull(payload);
        Assert.False(payload.Success);
    }

    [Fact]
    public async Task CreateBook_ThenGetById_ReturnsCreatedBook()
    {
        var client = CreateClient();

        var book = new Book
        {
            Title = "New Test Book",
            Author = "Test Author",
            ISBN = "111-1234567890",
            PublicationYear = 2024,
            Genre = "Test Genre",
            IsAvailable = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/books", book);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdPayload = await createResponse.Content.ReadFromJsonAsync<ApiResponse<Book>>();

        Assert.NotNull(createdPayload);
        Assert.True(createdPayload.Success);
        Assert.NotNull(createdPayload.Data);

        var createdId = createdPayload.Data!.Id;

        var getResponse = await client.GetAsync($"/api/books/{createdId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var getPayload = await getResponse.Content.ReadFromJsonAsync<ApiResponse<Book>>();

        Assert.NotNull(getPayload);
        Assert.True(getPayload.Success);
        Assert.NotNull(getPayload.Data);
        Assert.Equal("New Test Book", getPayload.Data!.Title);
        Assert.Equal("111-1234567890", getPayload.Data.ISBN);
    }

    [Fact]
    public async Task UpdateBook_ThenGetById_ReturnsUpdatedBook()
    {
        var client = CreateClient();

        var update = new Book
        {
            Title = "Updated Title",
            Author = "Updated Author",
            ISBN = "222-1234567890",
            PublicationYear = 2020,
            Genre = "Updated Genre",
            IsAvailable = false
        };

        var updateResponse = await client.PutAsJsonAsync("/api/books/1", update);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedPayload = await updateResponse.Content.ReadFromJsonAsync<ApiResponse<Book>>();

        Assert.NotNull(updatedPayload);
        Assert.True(updatedPayload.Success);
        Assert.NotNull(updatedPayload.Data);
        Assert.Equal("Updated Title", updatedPayload.Data!.Title);
        Assert.Equal("222-1234567890", updatedPayload.Data.ISBN);

        var getResponse = await client.GetAsync("/api/books/1");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var getPayload = await getResponse.Content.ReadFromJsonAsync<ApiResponse<Book>>();

        Assert.NotNull(getPayload);
        Assert.True(getPayload.Success);
        Assert.NotNull(getPayload.Data);
        Assert.Equal("Updated Title", getPayload.Data!.Title);
        Assert.Equal("222-1234567890", getPayload.Data.ISBN);
    }

    [Fact]
    public async Task DeleteBook_ThenGetById_Returns404()
    {
        var client = CreateClient();

        var deleteResponse = await client.DeleteAsync("/api/books/1");

        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var deletePayload = await deleteResponse.Content.ReadFromJsonAsync<ApiResponse>();

        Assert.NotNull(deletePayload);
        Assert.True(deletePayload.Success);

        var getResponse = await client.GetAsync("/api/books/1");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task GetAllBooks_WithAuthorFilter_ReturnsFiltered()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/books?author=tolk");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Book>>>();

        Assert.NotNull(payload);
        Assert.True(payload.Success);
        Assert.NotNull(payload.Data);

        var list = payload.Data!.ToList();

        Assert.Single(list);
        Assert.Contains("Tolkien", list[0].Author, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetAllBooks_WithSortByTitle_ReturnsSorted()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/books?sortBy=title");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Book>>>();

        Assert.NotNull(payload);
        Assert.True(payload.Success);
        Assert.NotNull(payload.Data);

        var list = payload.Data!.ToList();

        Assert.Equal(3, list.Count);
        Assert.Equal("1984", list[0].Title);
        Assert.Equal("Pride and Prejudice", list[1].Title);
        Assert.Equal("The Lord of the Rings", list[2].Title);
    }
}