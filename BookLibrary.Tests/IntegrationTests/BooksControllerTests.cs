using System.Net;
using System.Net.Http.Json;
using BookLibrary.Data.Requests.Book;
using BookLibrary.Data.Responses;
using BookLibrary.Models;

namespace BookLibrary.Tests.IntegrationTests;

[Collection("TestContainers")]
[Trait("Category", "Integration")]
public class BooksControllerTests(BookLibraryTestContainersFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    
    // ========================================================================
    // Тесты ListBooksController
    // ========================================================================

    [Fact]
    public async Task GetBooks_ReturnsListOfBooks()
    {
        var response = await _client.GetAsync("/api/books");
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        var ids = apiResponse?.Data?.Select(b => b.Id).ToList();
        
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);
        
        // Проверяем, что данные по умолчанию отсортированы по id по возрастанию
        var sortedIds = ids?.OrderBy(id => id).ToList();
        Assert.Equal(sortedIds, ids);
    }

    [Fact]
    public async Task GetBooks_WithWithAuthorsAndWithCategories_ReturnsBooksIncludingRelatedData()
    {
        var response = await _client.GetAsync("/api/books?withAuthors=true&withCategories=true");
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);

        // Проверка наличия связанных данных
        foreach (var book in apiResponse.Data)
        {
            Assert.NotNull(book.Author);
            Assert.NotNull(book.Category);
            Assert.True(book.Author.Id > 0);
            Assert.True(book.Category.Id > 0);
            Assert.False(string.IsNullOrWhiteSpace(book.Author.Name));
            Assert.False(string.IsNullOrWhiteSpace(book.Category.Name));
        }
    }

    [Fact]
    public async Task GetBooks_WithSortByTitleAsc_ReturnsSortedBooks()
    {
        var response = await _client.GetAsync("/api/books?sortBy=title");
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);

        // Проверка сортировки
        var titles = apiResponse.Data.Select(b => b.Title).ToList();
        var sortedTitles = titles.OrderBy(t => t).ToList();
        Assert.Equal(sortedTitles, titles);
    }

    [Fact]
    public async Task GetBooks_WithSortByPublicationYearDesc_ReturnsSortedBooks()
    {
        var response = await _client.GetAsync("/api/books?sortByDesc=publicationYear");
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data);

        var years = apiResponse.Data.Select(b => b.PublicationYear).ToList();
        var sortedYears = years.OrderByDescending(y => y).ToList();
        Assert.Equal(sortedYears, years);
    }
    
    [Fact]
    public async Task GetBooks_ReturnsListOfBooks_WithPagination()
    {
        // Act
        // Выполняем GET-запрос к эндпоинту с параметрами пагинации
        var response = await _client.GetAsync("/api/books?page=1&PageSize=5");
        
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
        
        // Проверяем, что размер страницы не превышает запрошенный
        Assert.True(apiResponse.Data.Count <= 5);
    }

    // ========================================================================
    // Тесты ShowBooksController
    // ========================================================================

    [Fact]
    public async Task GetBookById_ExistingId_ReturnsBook()
    {
        var listResponse = await _client.GetAsync("/api/books");
        listResponse.EnsureSuccessStatusCode();
        
        var list = await listResponse.Content.ReadFromJsonAsync<ApiResponseDto<List<Book>>>();
        Assert.NotNull(list);
        Assert.NotNull(list.Data);
        
        var books = list.Data; 
        Assert.NotEmpty(books);
        
        var bookId = books.First().Id;
        var response = await _client.GetAsync($"/api/books/{bookId}");
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.NotNull(apiResponse);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(bookId, apiResponse.Data.Id);
        Assert.False(string.IsNullOrWhiteSpace(apiResponse.Data.Title));
    }
    
    // ========================================================================
    // Тесты StoreBooksController
    // ========================================================================


    [Fact]
    public async Task CreateBook_ValidRequest_ReturnsCreatedBook()
    {
        var request = new CreateBookRequest
        {
            Title = "Integration Test Book",
            AuthorId = 1, // Из сидера
            CategoryId = 1, // Из сидера
            ISBN = "978-51-2345-678-9",
            PublicationYear = 2026,
            Genre = "Тест",
            IsAvailable = true
        };

        var response = await _client.PostAsJsonAsync("/api/books", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.NotNull(apiResponse);
        Assert.NotNull(apiResponse.Data);
        Assert.True(apiResponse.Success);
        Assert.Equal("Integration Test Book", apiResponse.Data.Title);
        Assert.Equal(2026, apiResponse.Data.PublicationYear);
    }

    // ========================================================================
    // Тесты UpdateBooksController
    // ========================================================================

    
    [Fact]
    public async Task UpdateBook_ExistingId_ReturnsUpdatedBook()
    {
        // Создаём книгу для обновления
        var createReq = new CreateBookRequest
        {
            Title = "Book to Update",
            AuthorId = 1,
            CategoryId = 1,
            ISBN = "978-59-8765-432-1",
            PublicationYear = 2020,
            Genre = "Old Genre",
            IsAvailable = true
        };
        var createResp = await _client.PostAsJsonAsync("/api/books", createReq);
        var created = await createResp.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        var bookId = created?.Data?.Id;

        // Обновляем
        var updateReq = new UpdateBookRequest
        {
            Title = "Updated Title",
            AuthorId = 1,
            CategoryId = 1,
            ISBN = "978-59-8765-432-1", // Сохраняем уникальный ISBN
            PublicationYear = 2025,
            Genre = "New Genre",
            IsAvailable = false
        };
        var updateResp = await _client.PutAsJsonAsync($"/api/books/{bookId}", updateReq);
        Assert.Equal(HttpStatusCode.OK, updateResp.StatusCode);
        
        var updated = await updateResp.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        Assert.Equal("Updated Title", updated?.Data?.Title);
        Assert.Equal("New Genre", updated?.Data?.Genre);
        Assert.Equal(2025, updated?.Data?.PublicationYear);
        Assert.False(updated?.Data?.IsAvailable);
    }
    
    // ========================================================================
    // Тесты DeleteBooksController
    // ========================================================================


    [Fact]
    public async Task DeleteBook_ExistingId_ReturnsSuccessAndRemovesBook()
    {
        // Создаём книгу для удаления
        var createReq = new CreateBookRequest
        {
            Title = "Book to Delete",
            AuthorId = 1,
            CategoryId = 1,
            ISBN = "978-51-1112-222-3",
            PublicationYear = 2024,
            Genre = "ToDelete",
            IsAvailable = true
        };
        var createResp = await _client.PostAsJsonAsync("/api/books", createReq);
        var created = await createResp.Content.ReadFromJsonAsync<ApiResponseDto<Book>>();
        var bookId = created?.Data?.Id;

        // Удаляем
        var deleteResp = await _client.DeleteAsync($"/api/books/{bookId}");
        deleteResp.EnsureSuccessStatusCode();
        
        var deleteResult = await deleteResp.Content.ReadFromJsonAsync<ApiResponseDto<object>>();
        Assert.True(deleteResult!.Success);

        // Проверяем отсутствие
        var getResp = await _client.GetAsync($"/api/books/{bookId}");
        Assert.Equal(HttpStatusCode.NotFound, getResp.StatusCode);
    }
}
