using System.Collections.Concurrent;
using BookLibrary.Models;

namespace BookLibrary.Storage;

/// <summary>
/// Единое хранилище для всех in-memory коллекций.
/// Хранит данные во время выполнения приложения.
/// </summary>
public class InMemoryStore
{
    // Ключ: тип сущности, Значение: коллекция сущностей
    private readonly ConcurrentDictionary<Type, object?> _collections = new();
    
    // Инициализация коллекции при первом доступе
    public List<T> GetCollection<T>() where T : class
    {
        return (List<T>)_collections.GetOrAdd(typeof(T), _ => InitializeCollection<T>())!;
    }
    
    private List<T>? InitializeCollection<T>() where T : class
    {
        return typeof(T) switch
        {
            _ when typeof(T) == typeof(Author) => InitializeAuthors() as List<T>,
            _ when typeof(T) == typeof(Book) => InitializeBooks() as List<T>,
            _ => []
        };
    }
    
    private List<Author> InitializeAuthors()
    {
        return [
            new Author
            {
                Id = 1,
                Name = "Фёдор Достоевский",
                Country = "Россия",
                BirthYear = 1821,
                DeathYear = 1881,
                Biography = "Русский писатель, мыслитель, философ и публицист.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Author
            {
                Id = 2,
                Name = "Лев Толстой",
                Country = "Россия",
                BirthYear = 1828,
                DeathYear = 1910,
                Biography = "Один из самых известных писателей мира.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddYears(-3),
                UpdatedAt = DateTime.UtcNow.AddYears(-2)
            },
            new Author
            {
                Id = 3,
                Name = "Михаил Булгаков",
                Country = "Россия",
                BirthYear = 1891,
                DeathYear = 1940,
                Biography = "Советский писатель, драматург, театральный режиссёр.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddYears(-1),
                UpdatedAt = DateTime.UtcNow.AddMonths(-6)
            },
            new Author
            {
                Id = 4,
                Name = "Михаил Шолохов",
                Country = "Россия",
                BirthYear = 1905,
                DeathYear = 1984,
                Biography = "Советский писатель, драматург.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddMonths(-1)
            },
            new Author
            {
                Id = 5,
                Name = "Джордж Оруэлл",
                Country = "Великобритания",
                BirthYear = 1903,
                DeathYear = 1950,
                Biography = "Английский писатель и публицист.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-12),
                UpdatedAt = DateTime.UtcNow.AddMonths(-6)
            },
            new Author
            {
                Id = 6,
                Name = "Эрнест Хемингуэй",
                Country = "США",
                BirthYear = 1899,
                DeathYear = 1961,
                Biography = "Американский писатель, лауреат Нобелевской премии.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-18),
                UpdatedAt = DateTime.UtcNow.AddMonths(-12)
            },
            new Author
            {
                Id = 7,
                Name = "Антуан де Сент-Экзюпери",
                Country = "Франция",
                BirthYear = 1900,
                DeathYear = 1944,
                Biography = "Французский писатель, поэт, авиатор. Автор знаменитого 'Маленького принца'.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-10),
                UpdatedAt = DateTime.UtcNow.AddMonths(-5)
            },
            new Author
            {
                Id = 8,
                Name = "Агата Кристи",
                Country = "Великобритания",
                BirthYear = 1890,
                DeathYear = 1976,
                Biography = "Британская писательница, королева детективного жанра. Автор более 80 романов.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-14),
                UpdatedAt = DateTime.UtcNow.AddMonths(-7)
            },
            new Author
            {
                Id = 9,
                Name = "Марк Твен",
                Country = "США",
                BirthYear = 1835,
                DeathYear = 1910,
                Biography = "Американский писатель, журналист и юморист. Автор 'Приключений Тома Сойера'.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-20),
                UpdatedAt = DateTime.UtcNow.AddMonths(-10)
            },
            new Author
            {
                Id = 10,
                Name = "Александр Пушкин",
                Country = "Россия",
                BirthYear = 1799,
                DeathYear = 1837,
                Biography = "Великий русский поэт, основоположник современного русского литературного языка.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddYears(-5),
                UpdatedAt = DateTime.UtcNow.AddYears(-3)
            },
            new Author
            {
                Id = 11,
                Name = "Франц Кафка",
                Country = "Чехия",
                BirthYear = 1883,
                DeathYear = 1924,
                Biography = "Чешский писатель немецкоязычной литературы. Автор 'Превращения' и 'Процесса'.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-16),
                UpdatedAt = DateTime.UtcNow.AddMonths(-8)
            }
        ];
    }
    
    private List<Book> InitializeBooks()
    {
        return [
            new Book
            {
                Id = 1,
                Title = "Преступление и наказание",
                Author = "Фёдор Достоевский",
                ISBN = "978-5-699-12345-6",
                PublicationYear = 1866,
                Genre = "Роман",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Book
            {
                Id = 2,
                Title = "Война и мир",
                Author = "Лев Толстой",
                ISBN = "978-5-17-012345-7",
                PublicationYear = 1869,
                Genre = "Роман",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-3),
                UpdatedAt = DateTime.UtcNow.AddYears(-2)
            },
            new Book
            {
                Id = 3,
                Title = "Мастер и Маргарита",
                Author = "Михаил Булгаков",
                ISBN = "978-5-699-54321-0",
                PublicationYear = 1967,
                Genre = "Фантастика",
                IsAvailable = false,
                CreatedAt = DateTime.UtcNow.AddYears(-1),
                UpdatedAt = DateTime.UtcNow.AddMonths(-6)
            },
            new Book
            {
                Id = 4,
                Title = "Анна Каренина",
                Author = "Лев Толстой",
                ISBN = "978-5-17-012346-4",
                PublicationYear = 1877,
                Genre = "Роман",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-4),
                UpdatedAt = DateTime.UtcNow.AddYears(-3)
            },
            new Book
            {
                Id = 5,
                Title = "1984",
                Author = "Джордж Оруэлл",
                ISBN = "978-0-452-28423-4",
                PublicationYear = 1949,
                Genre = "Антиутопия",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-8),
                UpdatedAt = DateTime.UtcNow.AddMonths(-4)
            },
            new Book
            {
                Id = 6,
                Title = "Старик и море",
                Author = "Эрнест Хемингуэй",
                ISBN = "978-0-684-80122-3",
                PublicationYear = 1952,
                Genre = "Повесть",
                IsAvailable = false,
                CreatedAt = DateTime.UtcNow.AddMonths(-15),
                UpdatedAt = DateTime.UtcNow.AddMonths(-9)
            },
            new Book
            {
                Id = 7,
                Title = "Тихий Дон",
                Author = "Михаил Шолохов",
                ISBN = "978-5-17-012347-1",
                PublicationYear = 1940,
                Genre = "Роман",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddMonths(-3)
            },
            new Book
            {
                Id = 8,
                Title = "Собачье сердце",
                Author = "Михаил Булгаков",
                ISBN = "978-5-699-54322-7",
                PublicationYear = 1925,
                Genre = "Сатира",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-1),
                UpdatedAt = DateTime.UtcNow.AddMonths(-2)
            }
        ];
    }
}
