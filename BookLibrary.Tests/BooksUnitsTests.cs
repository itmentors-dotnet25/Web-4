using BookLibrary.Models;
using BookLibrary.Requests;
using BookLibrary.Services;
using FluentAssertions;
using Xunit;

namespace BookLibrary.Tests.UnitTests.Services
{
    public class BookServiceTests
    {
        private readonly BookService _service;
        private readonly List<Book> _testBooks;

        public BookServiceTests()
        {
            // Инициализируем сервис с тестовыми данными
            _service = new BookService();

            // Получаем доступ к приватному списку через рефлексию или используем публичный метод
            // Для простоты создадим тестовые данные
            _testBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Война и мир", Author = "Лев Толстой", ISBN = "978-5-699-12345-6", PublicationYear = 1869},
                new Book { Id = 2, Title = "Анна Каренина", Author = "Лев Толстой", ISBN = "978-5-699-23456-7", PublicationYear = 1877},
                new Book { Id = 3, Title = "Преступление и наказание", Author = "Фёдор Достоевский", ISBN = "978-5-699-34567-8", PublicationYear = 1},
                new Book { Id = 4, Title = "Идиот", Author = "Фёдор Достоевский", ISBN = "978-5-699-45678-9", PublicationYear = 1869},
                new Book { Id = 5, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", ISBN = "978-5-699-56789-0", PublicationYear = 1967}
            };
        }

        #region GetAll Tests

        [Fact]
        public void GetAll_WithoutFilter_ReturnsAllBooks()
        {
            // Arrange
            var filter = new BookFilterRequest();

            // Act
            var result = _service.GetAll(filter);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(8);
        }

        [Fact]
        public void GetAll_WithAuthorFilter_ReturnsFilteredBooks()
        {
            // Arrange
            var filter = new BookFilterRequest { Author = "Толстой" };

            // Act
            var result = _service.GetAll(filter);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(b => b.Author.Contains("Толстой", StringComparison.OrdinalIgnoreCase))
                .Should().BeTrue();
        }

        [Fact]
        public void GetAll_WithAuthorFilter_CaseInsensitive()
        {
            // Arrange
            var filter = new BookFilterRequest { Author = "толстой" };

            // Act
            var result = _service.GetAll(filter);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetAll_WithAuthorFilter_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var filter = new BookFilterRequest { Author = "Шекспир" };

            // Act
            var result = _service.GetAll(filter);

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_ExistingBook_ReturnsBook()
        {
            // Arrange
            var bookId = 1;

            // Act
            var result = _service.GetBookById(bookId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.Title.Should().Be("Война и мир");
        }

        [Fact]
        public void GetById_NonExistingBook_ReturnsNull()
        {
            // Arrange
            var bookId = 999;

            // Act
            var result = _service.GetBookById(bookId);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Create Tests

        [Fact]
        public void Create_ValidBook_AddsBookToCollection()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Новая книга",
                Author = "Новый автор",
                ISBN = "978-5-999-99999-9",
                PublicationYear = 2024
            };

            var initialCount = _service.GetAll(new BookFilterRequest()).Count;

            // Act
            var result = _service.CreateBook(newBook);
            var allBooks = _service.GetAll(new BookFilterRequest());

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Title.Should().Be(newBook.Title);
            allBooks.Should().HaveCount(initialCount + 1);
        }

        [Fact]
        public void Create_MultipleBooks_AssignsUniqueIds()
        {
            // Arrange
            var book1 = new Book
            {
                Title = "Книга 1",
                Author = "Автор 1",
                ISBN = "978-5-111-11111-1",
                PublicationYear = 2020,
            };

            var book2 = new Book
            {
                Title = "Книга 2",
                Author = "Автор 2",
                ISBN = "978-5-222-22222-2",
                PublicationYear = 2021,
            };

            // Act
            var result1 = _service.CreateBook(book1);
            var result2 = _service.CreateBook(book2);

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();

            result1.Id.Should().BeGreaterThan(0, "Id должен быть больше 0");
            result2.Id.Should().BeGreaterThan(0, "Id должен быть больше 0");

            result1.Id.Should().NotBe(result2.Id, "Id должны быть уникальными");

            // Дополнительная проверка
            result1.Id.Should().Be(_service.GetAll(new BookFilterRequest()).Max(b => b.Id) - 1);
            result2.Id.Should().Be(_service.GetAll(new BookFilterRequest()).Max(b => b.Id));
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_ExistingBook_UpdatesBook()
        {
            // Arrange
            var bookId = 1;
            var updatedBook = new Book
            {
                Title = "Обновлённая книга",
                Author = "Обновлённый автор",
                ISBN = "978-5-699-12345-6",
                PublicationYear = 2024
            };

            // Act
            var result = _service.UpdateBook(bookId, updatedBook);
            var retrieved = _service.GetBookById(bookId);

            // Assert
            result.Should().NotBeNull();
            retrieved.Title.Should().Be("Обновлённая книга");
            retrieved.Author.Should().Be("Обновлённый автор");
            retrieved.PublicationYear.Should().Be(2024);
        }

        [Fact]
        public void Update_NonExistingBook_ReturnsNull()
        {
            // Arrange
            var bookId = 999;
            var updatedBook = new Book
            {
                Title = "Не существующая книга",
                Author = "Автор",
                ISBN = "978-5-999-99999-9",
                PublicationYear = 2024
            };

            // Act
            var result = _service.UpdateBook(bookId, updatedBook);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_ExistingBook_RemovesBook()
        {
            // Arrange
            var bookId = 1;
            var initialCount = _service.GetAll(new BookFilterRequest()).Count;

            // Act
            var result = _service.DeleteBook(bookId);
            var allBooks = _service.GetAll(new BookFilterRequest());
            var deletedBook = _service.GetBookById(bookId);

            // Assert
            result.Should().BeTrue();
            allBooks.Should().HaveCount(initialCount - 1);
            deletedBook.Should().BeNull();
        }

        [Fact]
        public void Delete_NonExistingBook_ReturnsFalse()
        {
            // Arrange
            var bookId = 999;

            // Act
            var result = _service.DeleteBook(bookId);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Combined Scenarios

        [Fact]
        public void CreateThenDelete_BookIsRemoved()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Временная книга",
                Author = "Тест",
                ISBN = "978-5-888-88888-8",
                PublicationYear = 2024,
            };

            // Act
            var created = _service.CreateBook(newBook);
            var deleted = _service.DeleteBook(created.Id);
            var retrieved = _service.GetBookById(created.Id);

            // Assert
            deleted.Should().BeTrue();
            retrieved.Should().BeNull();
        }

        #endregion
    }
}