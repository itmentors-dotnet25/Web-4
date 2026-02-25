using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace BookLibrary.Tests.Controllers;

public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllBooks_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/books");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Book>>>();

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data!);
    }
}