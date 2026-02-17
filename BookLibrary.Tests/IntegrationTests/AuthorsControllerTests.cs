using System.Net.Http.Json;
using BookLibrary.Data.Responses;
using BookLibrary.Models;

namespace BookLibrary.Tests.IntegrationTests;

[Collection("TestContainers")]
[Trait("Category", "Integration")]
public class AuthorsControllerTests(BookLibraryTestContainersFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    
    // ========================================================================
    // Тесты ListAuthorsController
    // ========================================================================

    [Fact]
    public async Task GetAuthors_ReturnsListOfAuthors()
    {
        // Act
        // Выполняем GET-запрос к эндпоинту с параметрами сортировки и пагинации
        var response = await _client.GetAsync("/api/authors?sortBy=Name&PageSize=5");
        
        // Assert
        // Проверяем, что ответ успешный
        response.EnsureSuccessStatusCode();
        
        // Десериализуем JSON-ответ в общий DTO-обертку
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Author>>>();

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);
        
        // Проверяем сообщение об успехе
        Assert.Equal("Operation completed successfully", apiResponse.Message);
        
        // Проверяем, что данные отсортированы по имени по возрастанию
        for (int i = 0; i < apiResponse.Data.Count - 1; i++)
        {
            Assert.True(string.Compare(apiResponse.Data[i].Name, apiResponse.Data[i + 1].Name, StringComparison.Ordinal) <= 0);
        }
        
        // Проверяем, что размер страницы не превышает запрошенный
        Assert.True(apiResponse.Data.Count <= 5);
    }
    
    // ========================================================================
    // Тесты AuthorBooksController
    // ========================================================================
    
    [Fact]
    public async Task GetAuthorBooks_ReturnsBooks_WithRelatedData()
    {
        // Arrange
        // Сначала получаем список всех авторов, чтобы выбрать существующий ID
        var listResponse = await _client.GetAsync("/api/authors");
        listResponse.EnsureSuccessStatusCode();
        var listApiResponse = await listResponse.Content.ReadFromJsonAsync<ApiResponseDto<List<Author>>>();
        Assert.NotNull(listApiResponse?.Data);
        Assert.NotEmpty(listApiResponse.Data);
    
        var authorId = listApiResponse.Data.First().Id; // Берем ID первого автора

        // Act
        // Выполняем GET-запрос с флагами для подгрузки связанных данных
        var response = await _client.GetAsync($"/api/authors/{authorId}/books?withAuthors=true&withCategories=true");

        // Assert
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        // Проверяем, что хотя бы одна книга была найдена
        Assert.True(apiResponse.Data.Count > 0);

        // Проверяем, что для каждой книги загружены связанные Author и Category
        foreach (var book in apiResponse.Data)
        {
            Assert.NotNull(book.Author);
            Assert.NotNull(book.Category);
        
            // Проверяем, что идентификаторы совпадают
            Assert.Equal(authorId, book.Author.Id);
            Assert.True(!string.IsNullOrEmpty(book.Title));
            Assert.True(book.PublicationYear > 1000);
        }
    }
}
