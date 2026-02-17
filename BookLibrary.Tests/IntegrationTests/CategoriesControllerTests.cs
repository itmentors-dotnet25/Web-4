using System.Net.Http.Json;
using BookLibrary.Data.Responses;
using BookLibrary.Data.Responses.Categories;

namespace BookLibrary.Tests.IntegrationTests;

[Collection("TestContainers")]
[Trait("Category", "Integration")]
public class CategoriesControllerTests(BookLibraryTestContainersFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetCategoryStatistics_ReturnsStats_WithoutBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/categories/statistics"); // Без флага
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<CategoryStatisticsDto>>>();

        // Assert
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);

        // Проверяем, что поле Books в каждом элементе статистики равно null
        foreach (var stat in apiResponse.Data)
        {
            Assert.NotNull(stat.CategoryName);
            Assert.True(stat.BookCount >= 0);
            Assert.True(stat.AveragePublicationYear > 0);
            Assert.Null(stat.Books); // Основная проверка: связи не должны быть загружены
        }
    }

    [Fact]
    public async Task GetCategoryStatistics_ReturnsStats_WithBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/categories/statistics?withBooks=true"); // С флагом
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<CategoryStatisticsDto>>>();

        // Assert
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);

        // Проверяем, что поле Books теперь заполнено
        foreach (var stat in apiResponse.Data)
        {
            Assert.NotNull(stat.Books);
            Assert.NotEmpty(stat.Books);
            
            // Проверяем структуру данных внутри списка книг
            foreach (var book in stat.Books)
            {
                Assert.True(book.Id > 0);
                Assert.True(!string.IsNullOrEmpty(book.Title));
                Assert.True(book.PublicationYear > 0);
            }
        }
    }
}
