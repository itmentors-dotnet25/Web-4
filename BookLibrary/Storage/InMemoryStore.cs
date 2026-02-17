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
            _ when typeof(T) == typeof(Category) => InitializeCategories() as List<T>,
            _ when typeof(T) == typeof(Author) => InitializeAuthors() as List<T>,
            _ when typeof(T) == typeof(Book) => InitializeBooks() as List<T>,
            _ => []
        };
    }
    
    private List<Category> InitializeCategories()
    {
        return
        [
            new Category
            {
                Id = 1,
                Name = "Классическая литература",
                Description = "Великие произведения мировой литературы",
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Category
            {
                Id = 2,
                Name = "Фантастика",
                Description = "Научная фантастика и фэнтези",
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Category
            {
                Id = 3,
                Name = "Детектив",
                Description = "Детективные романы и триллеры",
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Category
            {
                Id = 4,
                Name = "Приключения",
                Description = "Приключенческие романы",
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Category
            {
                Id = 5,
                Name = "Антиутопия",
                Description = "Антиутопические произведения",
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            },
            new Category
            {
                Id = 6,
                Name = "Сатира",
                Description = "Сатирические произведения",
                CreatedAt = DateTime.UtcNow.AddYears(-2),
                UpdatedAt = DateTime.UtcNow.AddYears(-1)
            }
        ];
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
                AuthorId = 1,
                CategoryId = 1,
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
                AuthorId = 2,
                CategoryId = 1,
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
                AuthorId = 3,
                CategoryId = 2,
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
                AuthorId = 2,
                CategoryId = 1,
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
                AuthorId = 5,
                CategoryId = 5,
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
                AuthorId = 6,
                CategoryId = 1,
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
                AuthorId = 4,
                CategoryId = 1,
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
                AuthorId = 3,
                CategoryId = 6,
                ISBN = "978-5-699-54322-7",
                PublicationYear = 1925,
                Genre = "Сатира",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-1),
                UpdatedAt = DateTime.UtcNow.AddMonths(-2)
            },
            new Book
            {
                Id = 9,
                Title = "Маленький принц",
                AuthorId = 7,
                CategoryId = 1,
                ISBN = "978-0-15-601219-5",
                PublicationYear = 1943,
                Genre = "Повесть",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-10),
                UpdatedAt = DateTime.UtcNow.AddMonths(-8)
            },
            new Book
            {
                Id = 10,
                Title = "Убийство в Восточном экспрессе",
                AuthorId = 8,
                CategoryId = 3,
                ISBN = "978-0-00-711930-3",
                PublicationYear = 1934,
                Genre = "Детектив",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-15),
                UpdatedAt = DateTime.UtcNow.AddMonths(-10)
            },
            new Book
            {
                Id = 11,
                Title = "Приключения Тома Сойера",
                AuthorId = 9,
                CategoryId = 4,
                ISBN = "978-0-14-243707-5",
                PublicationYear = 1876,
                Genre = "Приключенческий роман",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-20),
                UpdatedAt = DateTime.UtcNow.AddMonths(-15)
            },
            new Book
            {
                Id = 12,
                Title = "Евгений Онегин",
                AuthorId = 10,
                CategoryId = 1,
                ISBN = "978-5-08-004567-8",
                PublicationYear = 1833,
                Genre = "Роман в стихах",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddYears(-5),
                UpdatedAt = DateTime.UtcNow.AddYears(-4)
            },
            new Book
            {
                Id = 13,
                Title = "Превращение",
                AuthorId = 11,
                CategoryId = 2,
                ISBN = "978-0-525-56470-2",
                PublicationYear = 1915,
                Genre = "Новелла",
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow.AddMonths(-16),
                UpdatedAt = DateTime.UtcNow.AddMonths(-8)
            }
        ];
    }
}
