// BookLibrary.Tests/UnitTests/BookServiceTests.cs
using Xunit;
using BookLibrary.Services;
using BookLibrary.Models;
using BookLibrary.Requests;
using System.Linq;

namespace BookLibrary.Tests;

public class BookServiceTests
{
    private readonly BookService _service;

    public BookServiceTests()
    {
        // Создаём новый экземпляр сервиса для каждого теста
        _service = new BookService();
    }

    // ============ ТЕСТЫ ДЛЯ GetAll ============

    [Fact]
    public void GetAll_WithoutFilter_ReturnsAllBooks()
    {
        // Act
        var result = _service.GetAll(new BookFilterRequest());

        // Assert
        Assert.Equal(8, result.Count);
        Assert.Contains(result, b => b.Title == "Война и мир");
        Assert.Contains(result, b => b.Author == "Лев Толстой");
        Assert.Contains(result, b => b.Author == "Фёдор Достоевский");
    }

    [Fact]
    public void GetAll_WithAuthorFilter_ReturnsFilteredBooks()
    {
        // Arrange
        var filter = new BookFilterRequest { Author = "Толстой" };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, book =>
            Assert.Contains("Толстой", book.Author, System.StringComparison.OrdinalIgnoreCase));
        Assert.Contains(result, b => b.Title == "Война и мир");
        Assert.Contains(result, b => b.Title == "Анна Каренина");
    }

    [Fact]
    public void GetAll_WithAuthorFilter_CaseInsensitive()
    {
        // Arrange
        var filterLower = new BookFilterRequest { Author = "толстой" };
        var filterUpper = new BookFilterRequest { Author = "ТОЛСТОЙ" };

        // Act
        var resultLower = _service.GetAll(filterLower);
        var resultUpper = _service.GetAll(filterUpper);

        // Assert
        Assert.Equal(2, resultLower.Count);
        Assert.Equal(2, resultUpper.Count);
    }

    [Fact]
    public void GetAll_WithNonExistentAuthor_ReturnsEmptyList()
    {
        // Arrange
        var filter = new BookFilterRequest { Author = "Неизвестный автор" };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_WithSortByTitleAsc_ReturnsSortedBooks()
    {
        // Arrange
        var filter = new BookFilterRequest { SortBy = "title", SortOrder = "asc" };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Equal(8, result.Count);
        Assert.Equal("Анна Каренина", result[0].Title); // Первая по алфавиту
        Assert.Equal("Война и мир", result[1].Title);
    }

    [Fact]
    public void GetAll_WithSortByTitleDesc_ReturnsReverseSortedBooks()
    {
        // Arrange
        var filter = new BookFilterRequest { SortBy = "title", SortOrder = "desc" };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Equal(8, result.Count);
        Assert.Equal("Собачье сердце", result[0].Title); // Последняя по алфавиту
        Assert.Equal("Преступление и наказание", result[1].Title);
    }

    [Fact]
    public void GetAll_WithEmptyAuthorFilter_ReturnsAllBooks()
    {
        // Arrange
        var filter = new BookFilterRequest { Author = "" };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Equal(8, result.Count);
    }

    [Fact]
    public void GetAll_WithWhitespaceAuthorFilter_ReturnsAllBooks()
    {
        // Arrange
        var filter = new BookFilterRequest { Author = "   " };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Equal(8, result.Count);
    }

    // ============ ТЕСТЫ ДЛЯ GetBookById ============

    [Fact]
    public void GetBookById_ExistingId_ReturnsBook()
    {
        // Act
        var result = _service.GetBookById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Война и мир", result.Title);
        Assert.Equal("Лев Толстой", result.Author);
    }

    [Fact]
    public void GetBookById_NonExistentId_ReturnsNull()
    {
        // Act
        var result = _service.GetBookById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetBookById_ZeroId_ReturnsNull()
    {
        // Act
        var result = _service.GetBookById(0);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1, "Война и мир", "Лев Толстой")]
    [InlineData(3, "Преступление и наказание", "Фёдор Достоевский")]
    [InlineData(5, "Мастер и Маргарита", "Михаил Булгаков")]
    [InlineData(8, "Евгений Онегин", "Александр Пушкин")]
    public void GetBookById_MultipleIds_ReturnsCorrectBooks(int id, string expectedTitle, string expectedAuthor)
    {
        // Act
        var result = _service.GetBookById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(expectedTitle, result.Title);
        Assert.Equal(expectedAuthor, result.Author);
    }

    // ============ ТЕСТЫ ДЛЯ CreateBook ============

    [Fact]
    public void CreateBook_ValidBook_IncrementsIdAndAddsToCollection()
    {
        // Arrange
        var initialCount = _service.GetAll(new BookFilterRequest()).Count;
        var newBook = new Book
        {
            Title = "Новая книга",
            Author = "Новый автор",
            ISBN = "978-5-00000-000-0",
            PublicationYear = 2024
        };

        // Act
        var result = _service.CreateBook(newBook);
        var allBooks = _service.GetAll(new BookFilterRequest());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(initialCount + 1, allBooks.Count);
        Assert.Equal(initialCount + 1, result.Id); // Следующий ID после 8 = 9
        Assert.Equal("Новая книга", result.Title);
        Assert.Equal("Новый автор", result.Author);
        Assert.Contains(allBooks, b => b.Id == result.Id);
    }

    [Fact]
    public void CreateBook_MultipleBooks_AssignsUniqueIncrementalIds()
    {
        // Arrange
        var initialMaxId = _service.GetAll(new BookFilterRequest()).Max(b => b.Id);

        // Act
        var book1 = _service.CreateBook(new Book { Title = "Книга 1", Author = "Автор 1", ISBN = "1", PublicationYear = 2024 });
        var book2 = _service.CreateBook(new Book { Title = "Книга 2", Author = "Автор 2", ISBN = "2", PublicationYear = 2024 });
        var book3 = _service.CreateBook(new Book { Title = "Книга 3", Author = "Автор 3", ISBN = "3", PublicationYear = 2024 });

        // Assert
        Assert.Equal(initialMaxId + 1, book1.Id);
        Assert.Equal(initialMaxId + 2, book2.Id);
        Assert.Equal(initialMaxId + 3, book3.Id);
    }

    [Fact]
    public void CreateBook_WithEmptyTitle_CreatesBookWithoutValidation()
    {
        // Arrange
        // Важно: сервис НЕ должен выполнять валидацию — это задача контроллера/валидатора
        var newBook = new Book
        {
            Title = "", // Пустое название
            Author = "Автор",
            ISBN = "978-5-00000-000-0",
            PublicationYear = 2024
        };

        // Act
        var result = _service.CreateBook(newBook);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("", result.Title); // Сервис не валидирует — принимает любые данные
    }

    // ============ ТЕСТЫ ДЛЯ UpdateBook ============

    [Fact]
    public void UpdateBook_ExistingId_UpdatesBookProperties()
    {
        // Arrange
        var updatedBook = new Book
        {
            Id = 1,
            Title = "Обновлённое название",
            Author = "Обновлённый автор",
            ISBN = "978-5-99999-999-9",
            PublicationYear = 2023
        };

        // Act
        var result = _service.UpdateBook(1, updatedBook);
        var retrievedBook = _service.GetBookById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Обновлённое название", result.Title);
        Assert.Equal("Обновлённый автор", result.Author);
        Assert.Equal("978-5-99999-999-9", result.ISBN);
        Assert.Equal(2023, result.PublicationYear);

        // Проверяем, что изменения сохранились в коллекции
        Assert.NotNull(retrievedBook);
        Assert.Equal("Обновлённое название", retrievedBook.Title);
    }

    [Fact]
    public void UpdateBook_NonExistentId_ReturnsNull()
    {
        // Arrange
        var book = new Book
        {
            Id = 999,
            Title = "Несуществующая",
            Author = "Автор",
            ISBN = "978-5-00000-000-0",
            PublicationYear = 2024
        };

        // Act
        var result = _service.UpdateBook(999, book);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateBook_NullBook_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.UpdateBook(1, null));
    }

    [Fact]
    public void UpdateBook_PartialUpdate_OnlyChangesSpecifiedProperties()
    {
        // Arrange
        // Получаем оригинальную книгу для сравнения
        var original = _service.GetBookById(2);
        Assert.NotNull(original);

        var originalTitle = original.Title;

        // Создаём обновлённую версию с изменённым только автором
        var updated = new Book
        {
            Id = 2,
            Title = originalTitle, // Оставляем без изменений
            Author = "Новый автор", // Меняем только автора
            ISBN = original.ISBN,
            PublicationYear = original.PublicationYear
        };

        // Act
        var result = _service.UpdateBook(2, updated);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(originalTitle, result.Title); // Название не изменилось
        Assert.Equal("Новый автор", result.Author); // Автор изменился
    }

    // ============ ТЕСТЫ ДЛЯ DeleteBook ============

    [Fact]
    public void DeleteBook_ExistingId_RemovesBookAndReturnsTrue()
    {
        // Arrange
        var initialCount = _service.GetAll(new BookFilterRequest()).Count;

        // Act
        var result = _service.DeleteBook(1);
        var allBooks = _service.GetAll(new BookFilterRequest());
        var deletedBook = _service.GetBookById(1);

        // Assert
        Assert.True(result);
        Assert.Equal(initialCount - 1, allBooks.Count);
        Assert.Null(deletedBook); // Книга больше не существует
    }

    [Fact]
    public void DeleteBook_NonExistentId_ReturnsFalse()
    {
        // Act
        var result = _service.DeleteBook(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeleteBook_MultipleBooks_DeletesCorrectBook()
    {
        // Act
        var result1 = _service.DeleteBook(3); // Удаляем "Преступление и наказание"
        var result2 = _service.DeleteBook(5); // Удаляем "Мастер и Маргарита"

        // Assert
        Assert.True(result1);
        Assert.True(result2);

        var remainingBooks = _service.GetAll(new BookFilterRequest());
        Assert.DoesNotContain(remainingBooks, b => b.Id == 3);
        Assert.DoesNotContain(remainingBooks, b => b.Id == 5);
        Assert.Contains(remainingBooks, b => b.Id == 1); // Остальные книги на месте
        Assert.Contains(remainingBooks, b => b.Id == 2);
    }

    // ============ ИНТЕГРАЦИОННЫЕ ТЕСТЫ СЦЕНАРИЕВ ============

    [Fact]
    public void FullCRUDScenario_CreatesUpdatesAndDeletesBook()
    {
        // Arrange: Создаём новую книгу
        var newBook = new Book
        {
            Title = "Тестовая книга",
            Author = "Тестовый автор",
            ISBN = "978-5-11111-111-1",
            PublicationYear = 2024
        };

        // Act 1: Создание
        var created = _service.CreateBook(newBook);
        Assert.NotNull(created);
        Assert.Equal("Тестовая книга", created.Title);

        // Act 2: Получение по ID
        var retrieved = _service.GetBookById(created.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(created.Id, retrieved.Id);

        // Act 3: Обновление
        var updatedBook = new Book
        {
            Id = created.Id,
            Title = "Обновлённая книга",
            Author = "Обновлённый автор",
            ISBN = created.ISBN,
            PublicationYear = 2025
        };
        var updated = _service.UpdateBook(created.Id, updatedBook);
        Assert.NotNull(updated);
        Assert.Equal("Обновлённая книга", updated.Title);

        // Act 4: Удаление
        var deleted = _service.DeleteBook(created.Id);
        Assert.True(deleted);

        // Assert: Книга больше не существует
        var afterDelete = _service.GetBookById(created.Id);
        Assert.Null(afterDelete);
    }

    [Fact]
    public void FilterAndSortCombined_ReturnsCorrectResults()
    {
        // Arrange
        var filter = new BookFilterRequest
        {
            Author = "Достоевский",
            SortBy = "title",
            SortOrder = "asc"
        };

        // Act
        var result = _service.GetAll(filter);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Идиот", result[0].Title); // "Идиот" < "Преступление..."
        Assert.Equal("Преступление и наказание", result[1].Title);
        Assert.All(result, b =>
            Assert.Contains("Достоевский", b.Author, System.StringComparison.OrdinalIgnoreCase));
    }
}