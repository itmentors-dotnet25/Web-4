# BookLibrary - Реализация подключения к базе данных и расширение функциональности

## 📊 1. Настройка подключения к базе данных

### Конфигурация подключения
Реализована поддержка PostgreSQL с гибкой настройкой через конфигурационные файлы:

#### Конфигурация в `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=CSharpDB;Username=postgres;Password=password;Include Error Detail=true"
  },
  "Storage": {
    "Provider": "Postgres"  // или "InMemory"
  }
}
```

#### Ключевые параметры подключения:
- **Host** - адрес сервера базы данных
- **Port** - порт подключения (5432 для PostgreSQL)
- **Database** - имя базы данных
- **Username/Password** - учетные данные
- **Include Error Detail** - детализация ошибок для отладки

## 🔄 2. Гибкая система выбора хранилища

### Архитектура переключения хранилищ
Реализован механизм выбора типа хранилища на основе конфигурации:

#### Методы регистрации:
```csharp
// Регистрация InMemory хранилища
services.ConfigureInMemoryStorage();

// Регистрация PostgreSQL хранилища
services.ConfigurePostgresStorage(configuration);
```

#### Логика выбора:
```csharp
var storageProvider = configuration["Storage:Provider"];
if (storageProvider == "Postgres")
{
    services.ConfigurePostgresStorage(configuration);
}
else
{
    services.ConfigureInMemoryStorage();
}
```

### Преимущества подхода:
- **Гибкость** - быстрая смена хранилища через конфигурацию
- **Изоляция** - независимость бизнес-логики от типа хранилища
- **Тестируемость** - можно использовать InMemory для юнит-тестов

## 🗄️ 3. Управление миграциями и сидерами

### Работа с Entity Framework Core

#### Создание и применение миграций:
```bash
# Создание начальной схемы базы данных
dotnet ef migrations add InitialCreate

# Добавление миграции с начальными данными
dotnet ef migrations add SeedInitialData

# Обновление существующих данных
dotnet ef migrations add UpdateBookSeedData

# Применение миграций к базе данных
dotnet ef database update

# Откат к конкретной миграции
dotnet ef database update 20260215180422_SeedInitialData
```

### Система сидеров (Data Seeders)

#### Архитектура сидеров:
```csharp
// Базовый интерфейс сидера
public interface IDataSeeder
{
    Task SeedAsync(AppDbContext context);
}

// Фабрика для автоматического выполнения сидеров
public class SeederFactory
{
    public static void ApplySeeders(ModelBuilder modelBuilder)
    {
        // Автоматическое обнаружение и выполнение всех IDataSeeder
    }
}
```

#### Пример сидера для книг:
```csharp
public class BookSeeder : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Детектив"},
            new Category { Id = 2, Name = "Фэнтези"},
            new Category { Id = 3, Name = "Классика"},
            new Category { Id = 4, Name = "Научная фантастика"},
            new Category { Id = 5, Name = "Роман"}
        );
    }
}
```

### Пример структуры папки Migrations:

```
BookLibrary.Infrastructure/Data/Migrations/
├── 20250215120000_InitialCreate.cs
├── 20250215120000_InitialCreate.Designer.cs
├── 20250215180422_SeedInitialData.cs
├── 20250215180422_SeedInitialData.Designer.cs
├── 20250216103045_UpdateBookSeedData.cs
├── 20250216103045_UpdateBookSeedData.Designer.cs
└── AppDbContextModelSnapshot.cs
```

### Установка инструментов разработки .NET CLI Tools:
```bash
dotnet tool install --global dotnet-ef
```

## 📋 4. Разделение данных через DTO

### Проблема и решение
Для разделения данных запроса от модели предметной области реализована система DTO:

#### Проблема:
- Сложность разделения атрибутов для полей модели
- Разные наборы свойств для ввода (POST) и вывода (GET)
- Навигационные свойства не должны отображаться в Swagger при создании

#### Реализация:
```csharp
// DTO для создания книги
using System.ComponentModel.DataAnnotations;
using BookLibrary.Models;

namespace BookLibrary.Data.Requests.Book;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Genre { get; set; }
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Преобразует запрос в модель книги
    /// </summary>
    public Models.Book ToBook() => new()
    {
        Title = Title,
        AuthorId = AuthorId,
        CategoryId = CategoryId,
        ISBN = ISBN,
        PublicationYear = PublicationYear,
        Genre = Genre,
        IsAvailable = IsAvailable
    };
}


// Контроллер с использованием DTO
using BookLibrary.Contracts.Services;
using BookLibrary.Data.Requests.Book;
using BookLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers.Books;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
[Tags("Books")]
public class StoreBookController(IBookService bookService)
    : ApiControllerBase
{
    /// <summary>
    /// Добавить новую книгу
    /// </summary>
    /// <param name="request">Данные книги</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная книга</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromBody] CreateBookRequest request,
        CancellationToken cancellationToken = default)
    {
        var createdBook = await bookService.CreateBookAsync(request.ToBook(), cancellationToken);

        return Created(createdBook);
    }
}

```
#### Ключевые моменты:
- изменен атрибут [FromBody] в методе public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
- преобразование DTO  в модель request.ToBook()

### Управление навигационными свойствами:
```csharp
public class Book
{
    // Свойства модели
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    
    // Навигационные свойства
    [JsonIgnore]  // Для исключения из Swagger и запросов создания
    [ForeignKey(nameof(AuthorId))]
    public virtual Author? Author { get; set; }
    
    [JsonIgnore]
    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }
}
```

## ✅ 5. Расширенная система валидации

### Кастомные правила валидации
Реализованы специализированные правила для проверки существования связанных сущностей:

#### Добавлены новые Правила валидации:
```csharp
// Проверка существования автора
public class AuthorExistsRule : AbstractValidator<int>
{
    public AuthorExistsRule(IServiceProvider serviceProvider)
    {
        RuleFor(id => id)
            .MustAsync(async (authorId, cancellation) =>
            {
                // Получаем контекст только если он зарегистрирован
                var context = serviceProvider.GetService(typeof(AppDbContext));
                
                if (context == null)
                {
                    // InMemory режим — пропускаем проверку существования
                    return true;
                }
                
                var dbContext = (AppDbContext)context;
                
                return await dbContext.Authors
                    .AnyAsync(a => a.Id == authorId, cancellation);
            })
            .WithMessage("Автор с указанным ID не найден");
    }
}
// Проверка существования категории
public class CategoryExistsRule<T> : AbstractValidator<int>
{
    // Аналогичная реализация
}
```

#### Использование в валидаторах:
```csharp
public class BookValidator : AbstractValidator<Book>
{
    public BookValidator(IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("Автор книги обязателен для заполнения")
            .SetValidator(authorExistsRule)
            .WithName("authorId");
            
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Категория книги обязательна для заполнения")
            .SetValidator(categoryExistsRule)
            .WithName("categoryId");
    }
}
```

### Асинхронная валидация:
```csharp
// Изменение метода фильтра для поддержки асинхронной валидации в классе 
public class ValidationResponseFilter(IServiceProvider serviceProvider) : IAsyncActionFilter

protected override async Task OnActionExecutionAsync(
    ActionExecutingContext context, 
    ActionExecutionDelegate next)
{
    ...

    // ВЫЗЫВАЕМ ВАЛИДАТОР ТОЛЬКО ЕСЛИ НЕТ ОШИБОК ПРИВЯЗКИ
    if (hasBindingErrors) continue;
    
    // АСИНХРОННЫЙ ВЫЗОВ!
    var validationResult = await validator.ValidateAsync(new ValidationContext<object>(parameterValue));
    
    ...
}
```

## 🔍 6. Усовершенствованная система спецификаций

### Базовый класс для сортировки
Вынесена общая логика обработки сортировки в базовый класс:

```csharp
public abstract class MySpecification<T> : ISpecification<T>
{
    protected void ApplySorting(
        string? sortBy,
        string? sortByDesc,
        Expression<Func<T, object>>? defaultSort = null)
    {
        // Логика применения сортировки по полям
        if (!string.IsNullOrEmpty(sortBy))
        {
            OrderBy = GetSortExpression(sortBy);
            IsDescending = false;
        }
        else if (!string.IsNullOrEmpty(sortByDesc))
        {
            OrderBy = GetSortExpression(sortByDesc);
            IsDescending = true;
        }
        else if (defaultSort != null)
        {
            OrderBy = defaultSort;
        }
    }
    
    private Expression<Func<T, object>> GetSortExpression(string propertyName)
    {
        // Динамическое создание выражения сортировки
    }
}
```

### Параметры включения связанных данных
Реализована поддержка параметров для загрузки связанных сущностей:

```csharp
// Пример использования в спецификации книг
public class BookSpecification : MySpecification<Book>
{
    public BookSpecification(
        string? title = null,
        int? minYear = null,
        int? maxYear = null,
        bool withAuthors = false,
        bool withCategories = false)
    {
        // Критерии фильтрации
        
        if (withAuthors)
        {
            Includes.Add(x => x.Author);
            IncludeStrings.Add("Author");
        }
        
        if (withCategories)
        {
            Includes.Add(x => x.Category);
            IncludeStrings.Add("Category");
        }
    }
}
```

## ⚡ 7. Решение проблем с InMemory хранилищем

### Проблема с методом AddInclude
Метод `AddInclude` предназначен для EF Core и не работает с InMemory хранилищем:

#### Проблема:
- `AddInclude` работает только с `IQueryable`, поддерживающим отложенную загрузку
- В InMemory случае нужно вручную загружать связанные данные

#### Решение:
```csharp
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    public async Task<List<T>> ListAsync(ISpecification<T> specification)
    {
        var result = ApplySpecification(_collection.AsQueryable(), specification).ToList();
        
        // Ручная загрузка связанных данных ТОЛЬКО если запрошено
        if (specification.IncludeStrings.Any())
        {
            LoadRelatedData(result, specification.IncludeStrings);
        }
        
        // Очистка навигационных свойств перед возвратом
        ClearRelatedData(result);
        
        return result;
    }
    
    private void LoadRelatedData(List<T> entities, List<string> includeStrings)
    {
        // Ручная загрузка связанных данных из соответствующих коллекций
    }
    
    private void ClearRelatedData(List<T> entities)
    {
        // Очистка навигационных свойств для предотвращения утечки данных
        foreach (var entity in entities.OfType<Book>())
        {
            entity.Author = null;
            entity.Category = null;
        }
    }
}
```

## 🌐 8. Новые маршруты API

### Расширенная функциональность конечных точек

#### 1. Фильтрация и сортировка авторов:
```
GET /api/authors?Name=михаил&sortByDesc=birthYear
```
- **Фильтрация** по имени автора
- **Сортировка** по году рождения в порядке убывания

#### 2. Получение книг конкретного автора:
```
GET /api/authors/{id}/books
```
- Возвращает все книги указанного автора
- Поддерживает параметры фильтрации и сортировки

#### 3. Статистика по категориям:
```
GET /api/categories/statistics?withBooks=true
```
- Группировка книг по категориям
- Опциональное включение списка книг в каждой категории
- Статистика по количеству книг

## 🏗️ 9. Репозитории для работы с базой данных

### Реализация EfCore репозиториев
Созданы специализированные репозитории для работы с Entity Framework Core:

```csharp
// Репозиторий для книг
public class EfCoreBookRepository : EfCoreRepository<Book>, IBookRepository
{
    public EfCoreBookRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<List<Book>> GetByAuthorIdAsync(int authorId)
    {
        return await _context.Books
            .Where(b => b.AuthorId == authorId)
            .ToListAsync();
    }
}

// Репозиторий для авторов
public class EfCoreAuthorRepository : EfCoreRepository<Author>, IAuthorRepository
{
    // Специализированные методы для работы с авторами
}

// Репозиторий для категорий
public class EfCoreCategoryRepository : EfCoreRepository<Category>, ICategoryRepository
{
    // Специализированные методы для работы с категориями
}
```

## 🧪 10. Тестирование с использованием TestContainers

### Инфраструктура для интеграционных тестов
Реализована система тестирования с изолированными контейнерами базы данных:

#### Фабрика тестового окружения:
```csharp
public class BookLibraryTestContainersFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:15-alpine")
        .WithDatabase("test")
        .WithUsername("testuser")
        .WithPassword("password")
        .WithCleanUp(true)
        .Build();

    // Для отладки: публичное свойство строки подключения
    public string TestConnectionString => _dbContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        // 🔤 Исправляем кодировку КОНСОЛИ
        // 🔇 Отключаем логи Testcontainers
        // 🔒 КРИТИЧЕСКАЯ ПРОВЕРКА: Защита от случайного подключения к боевой БД
        // Дополнительная проверка: убеждаемся, что подключены к контейнеру
    }

    public new async Task DisposeAsync()
    {
        Console.WriteLine("🧹 Очистка: остановка и удаление контейнера...");
        await _dbContainer.DisposeAsync();
        Console.WriteLine("✅ Контейнер удален. БД полностью уничтожена.");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // 🔒 ШАГ 1: Явно устанавливаем окружение ТЕСТОВОЕ
        // 🔒 ШАГ 2: Переопределяем строку подключения ЧЕРЕЗ ПЕРЕМЕННУЮ ОКРУЖЕНИЯ (высший приоритет!)
        // 🔒 ШАГ 3: Дополнительно переопределяем через конфигурацию
        // 🔒 ШАГ 4: Заменяем контекст БД на тот, что использует строку из контейнера
        // 🔒 ШАГ 5: 🔇 Отключаем логи приложения (только Critical)
    }

```

#### Преимущества подхода:
- **Изоляция** - каждый тест работает с чистой базой данных
- **Реалистичность** - использование реальной PostgreSQL вместо InMemory
- **Автоматизация** - автоматическое создание и удаление контейнеров
- **Скорость** - быстрые тесты без настройки внешней инфраструктуры

### Пример теста:
```csharp
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
```

## 🎯 Заключение

Реализованная система предоставляет:

### Ключевые достижения:
1. **Гибкое хранилище** - быстрый переключатель между InMemory и PostgreSQL
2. **Управление миграциями** - полный контроль над схемой базы данных
3. **Автоматические сидеры** - удобное заполнение начальных данных
4. **Чистая архитектура** - разделение DTO и моделей предметной области
5. **Расширенная валидация** - проверка существования связанных сущностей
6. **Универсальные спецификации** - поддержка сортировки и включения связанных данных
7. **Решение проблем InMemory** - корректная работа с навигационными свойствами
8. **Расширенные API маршруты** - богатая функциональность конечных точек
9. **Специализированные репозитории** - оптимизированные запросы к базе данных
10. **Надежное тестирование** - изолированные тесты с TestContainers

### Технологический стек:
- **.NET 8** - основная платформа
- **Entity Framework Core** - ORM для работы с базой данных
- **PostgreSQL** - основная реляционная база данных
- **TestContainers** - изолированное тестирование
- **FluentValidation** - расширенная валидация
- **Swagger** - документация API
