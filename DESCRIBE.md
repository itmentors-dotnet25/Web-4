# BookLibrary - Архитектурное решение

## 📁 Структура проекта

```
BookLibrary/
├── Bootstrap/ # Механизм инициализации приложения
│ ├── Attributes/
│ │ └── BootstrapStepAttribute.cs
│ ├── AppSteps/ # Шаги для сконфигурированного приложения (WebApplication)
│ │ ├── MiddlewareSteps.cs
│ │ ├── RoutingSteps.cs
│ │ └── SwaggerSteps.cs
│ ├── BuilderSteps/ # Шаги для билдера (WebApplicationBuilder)
│ │ ├── ConfigurationSteps.cs
│ │ ├── DatabaseSteps.cs
│ │ ├── LoggingSteps.cs
│ │ └── ServiceRegistrationSteps.cs
│ └── Bootstrap.cs # Точка входа для инициализации
├── Config/ # Конфигурация приложения
├── Contracts/ # Интерфейсы и контракты
│ ├── Repositories/
│ ├── Services/
│ └── Specifications/
├── Controllers/ # Single Action Controllers
│ ├── Books/
│ │ ├── CreateBookController.cs
│ │ ├── ShowBookController.cs
│ │ ├── UpdateBookController.cs
│ │ ├── DeleteBookController.cs
│ │ └── ListBooksController.cs
│ └── Authors/
│ └── ...
├── Data/
│ └── Responses/ # Форматы ответов API
│ ├── ApiResponseDto.cs
│ ├── ApiErrorDto.cs
│ └── ErrorDetailDto.cs
├── Exceptions/ # Механизм обработки ошибок
│ ├── Base/ # Базовые исключения и провайдеры
│ │ ├── AppException.cs
│ │ ├── AttributeExceptionMetadataProvider.cs
│ │ ├── ExceptionHandler.cs
│ │ └── ExceptionMetadataAttribute.cs
│ ├── Contracts/ # Интерфейсы
│ │ ├── IExceptionMetadataProvider.cs
│ │ └── IExceptionHandlingFacade.cs
│ ├── Exceptions/ # Конкретные исключения
│ │ ├── BookNotFoundException.cs
│ │ ├── ForbiddenException.cs
│ │ ├── ModelNotFoundException.cs
│ │ └── ValidationException.cs
│ └── Handlers/ # Обработчики
├── Filters/
│ └── ValidationFilters/ # Валидация запросов
│ ├── ValidationResponseFilter.cs
│ └── ValidationResponseFormat.cs
├── Middleware/ # Custom middleware
│ ├── ExceptionHandlingMiddleware.cs
│ ├── RequestLoggingMiddleware.cs
│ └── RouteNotFoundMiddleware.cs
├── Models/ # Модели данных
│ ├── Book.cs
│ └── Author.cs
├── Repositories/ # Репозитории
│ ├── BookRepositories/
│ │ ├── IBookRepository.cs
│ │ └── MemoryBookRepository.cs
│ └── AuthorRepositories/
│ ├── IAuthorReadRepository.cs
│ └── MemoryAuthorReadRepository.cs
├── Services/ # Бизнес-логика
│ └── BookService.cs
├── Specifications/ # Спецификации для фильтрации
│ ├── Author/
│ │ ├── ArdalisAuthorSpecification.cs
│ │ └── AuthorFilterParams.cs
│ └── Book/
│ ├── MyBookSpecification.cs
│ └── BookFilterParams.cs
├── Storage/ # InMemory хранилище
│ └── InMemoryStore.cs
├── Validators/ # Валидаторы
│ ├── BookValidator.cs
│ └── Rules/
│ └── IsbnValidationRule.cs
├── Properties/
│ └── launchSettings.json
├── appsettings.json # Конфигурация
└── Program.cs # Точка входа приложения
│

├──
BookLibrary.Tests/               # Тесты
├──IntegrationTests
│ ├── AuthorsControllerTests.cs
│ ├── BooksControllerTests.cs
│ └── BookLibraryWebApplicationFactory.cs
└──UnitTests
  └── BookServiceTests.cs
```

## 🚀 1. Инициализация через Bootstrap и шаги

### Архитектура инициализации
Система использует декларативный подход к инициализации приложения через атрибуты `[BootstrapStep]`. Это позволяет:

- **Автоматическое обнаружение** шагов инициализации
- **Контроль порядка выполнения** через параметры атрибута
- **Разделение ответственности** между шагами

### Ключевые компоненты:
- **`BootstrapStepAttribute`** - атрибут для маркировки классов как шагов инициализации
- **`IBootstrapStep`** - интерфейс для шагов инициализации
- **`BootstrapRunner`** - исполнитель, который находит и запускает шаги в правильном порядке

### Порядок выполнения:
```csharp
[BootstrapStep(Order = 10)]  // Ранние шаги (конфигурация)
[BootstrapStep(Order = 20)]  // Основные шаги (сервисы)
[BootstrapStep(Order = 50)]  // Поздние шаги (middleware)
```
### Отключение шага в тестах:
```csharp
[BootstrapStep(skipInTests: true)]  // Отключение выполнения шага в тестах
```
### Инициализация:
```csharp
// В Program.cs
создается 
    builder = WebApplication.CreateBuilder
    Выполняются шаги для билдера — Регистрация сервисов, Настройка конфигурации, базы данных, логирования
затем создается приложение 
    app = builder.Build
    Выполняются шаги для приложения — КОНФИГУРАЦИЯ ПАЙПЛАЙНА
```

## 🛡️ 2. Гибкий механизм обработки исключений

### Архитектура ExceptionHandler
Централизованная система обработки исключений с настраиваемыми метаданными:

```
ExceptionHandler
    ├── IExceptionMetadataProvider
    ├── ExceptionMetadata
    └── ExceptionHandlingMiddleware
```

### Ключевые файлы:
- **`ExceptionHandler.cs`** - фасад для обработки исключений
- **`IExceptionMetadataProvider.cs`** - интерфейс для провайдеров метаданных
- **`ExceptionMetadata.cs`** - модель метаданных исключения
- **`ExceptionMetadataAttribute.cs`** - атрибут для декларативной настройки

### Приоритетность настроек:
1. **Атрибуты** на исключениях (наивысший приоритет)
2. **Конфигурация** в `appsettings.json`
3. **Провайдеры** через DI (низший приоритет)

### Конфигурация в appsettings.json:
```json
{
  "Logging": {
    "IncludeStackTraceInLogs": false //параметр включения трассировки логов
  },
  "ExceptionHandling": {
    "DefaultStatusCode": 500,
    "ShowStackTrace": false,
    "LogLevel": "Error",
    "CustomMappings": {
      "ValidationException": {
        "StatusCode": 400,
        "Message": "Ошибка валидации"
      }
    }
  }
}
```

## 📦 3. Единый формат ответов API

### Структура ответа:
```json
{
  "success": true,
  "data": { /* данные ответа */ },
  "errors": [
    {
      "code": "VALIDATION_ERROR",
      "message": "Описание ошибки",
      "details": { /* дополнительные детали */ }
    }
  ],
  "metadata": {
    "timestamp": "2024-01-01T00:00:00Z",
    "requestId": "guid",
    "pagination": {
      "page": 1,
      "pageSize": 20,
      "totalItems": 100,
      "totalPages": 5
    }
  }
}
```

### Реализация:
- **`ApiResponse<T>`** - обертка для всех ответов API
- **`ErrorResponse`** - стандартизированная модель ошибки
- **`ResponseMetadata`** - метаданные ответа (пагинация, временные метки)

## ✅ 4. Система валидации

### Используемые пакеты:
```xml
<PackageReference Include="FluentValidation" Version="12.1.1" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="12.1.1" />
```

### Архитектура:
```
Validation/
├── Validators/          # Валидаторы для моделей
├── Attributes/          # Кастомные атрибуты валидации
├── Filters/             # Фильтры для обработки ошибок
└── Extensions/          # Расширения для DI
```

### Ключевые компоненты:
- **`BookValidator`**, **`AuthorValidator`** - валидаторы для бизнес-моделей
- **`YearRangeAttribute`** - кастомный атрибут валидации диапазона годов
- **`ValidationResponseFilter`** - фильтр для обработки ошибок валидации

### Регистрация:
```csharp
используется шаг [BootstrapStep(51, "Configure validation behavior")]
    
-- Ключемые моменты --

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Отключает встроенную валидацию
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationResponseFilter>(); // Добавляется ко всем контроллерам
});

builder.Services.AddScoped<IValidator<Book>, BookValidator>(); // ← Валидатор для модели Book
```
### 🔍 Как это работает по шагам
1. Запрос приходит в контроллер (например, POST /api/books с моделью Book)
2. Фильтр ValidationResponseFilter перехватывает выполнение:
 - Получает параметр из ActionArguments (объект Book)
 - Находит валидатор IValidator<Book> через DI
 - Вызывает validator.Validate() для проверки бизнес-правил
3. Если есть ошибки:
 - Добавляются в ModelState
 - Формируется кастомный ответ через ValidationResponseFormat.FromModelStateErrors()
 - Возвращается статус 422 Unprocessable Entity

### 📦 Где определены правила валидации?
```csharp
public class BookValidator : AbstractValidator<Book>
{
public BookValidator()
{
RuleFor(x => x.Title)
.NotEmpty().WithMessage("Название обязательно")
.MaximumLength(200).WithMessage("Название не должно превышать 200 символов");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Автор обязателен");
            
        // ... другие правила
    }
}
```

### ✅ Итог

| Аспект            | Реализация                                        |
|:------------------|:--------------------------------------------------|
| Тип валидации     | FluentValidation (не DataAnnotations)             |
| Где правила       | BookValidator в папке Validators                  |
| Когда срабатывает | Фильтр ValidationResponseFilter на каждом запросе |
| Формат ответа     | Кастомный через ValidationResponseFormat          |
| Статус ошибки     | 422 Unprocessable Entity                          |

Валидация не использует атрибуты вроде [Required] в модели Book.
Все правила определены в отдельном классе-валидаторе через FluentValidation. 
Это более гибкий и тестируемый подход!
Если все-таки нужно использовать DataAnnotations - то необходимо выполнить 2 действия
1. В шаге инициализации [BootstrapStep(51, "Configure validation behavior")] отключить
```csharp
    // Регистрация валидатора для модели Book
    builder.Services.AddScoped<IValidator<Book>, BookValidator>();
```
   
2. Заполнить атрубуты полей в модели
   [Required(ErrorMessage = "ISBN обязателен для заполнения")]
🎯

## 🔍 5. Фильтрация и сортировка списков

### Два механизма спецификаций:

#### 1. Самонаписанные спецификации (`MySpecification<T>`)
```csharp
public class MySpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; }
    public Expression<Func<T, object>> OrderBy { get; set; }
    public bool IsDescending { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
```

#### 2. Ardalis.Specification
```xml
<PackageReference Include="Ardalis.Specification" Version="9.3.1" />
<PackageReference Include="Ardalis.Specification.EntityFrameworkCore" Version="9.3.1" />
```

### Расширения для IQueryable:
```csharp
public static IQueryable<T> ApplySpecification<T>(
    this IQueryable<T> query, 
    ISpecification<T> specification)
{
    // Применение критериев, сортировки, пагинации
    return query;
}
```

### Пример использования:
```csharp
var spec = new BookSpecification()
    .WithTitleContains("C#")
    .WithYearRange(2020, 2024)
    .OrderByTitle()
    .Paginate(page: 1, pageSize: 20);

var books = await repository.ListAsync(spec);
```

## 💾 6. Хранение данных в памяти

### Используемые пакеты:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.23" />
```

### Ключевые классы:
- **`InMemoryStore<T>`** - потокобезопасное хранилище в памяти

### Особенности реализации:
- **Потокобезопасность** через `ConcurrentDictionary`
- **Поддержка спецификаций** для фильтрации
- **Время жизни** - на протяжении работы приложения

### Конфигурация:
```csharp
services.AddSingleton<IInMemoryContext, InMemoryContext>();
services.AddScoped(typeof(IRepository<>), typeof(InMemoryRepository<>));
```

## 📚 7. Документация API с Swagger

### Настройка:
```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookLibrary API",
        Version = "v1",
        Description = "API для управления библиотекой книг"
    });
    
    // Кастомные настройки
    options.EnableAnnotations();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BookLibrary.xml"));
});
```

### Использование:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLibrary v1");
        options.RoutePrefix = "api-docs";
    });
}
```

## 🎯 8. Паттерн Single Action Controller

### Архитектура:
Каждый контроллер отвечает за одно действие, что обеспечивает:

- **SRP** - один контроллер = одна ответственность
- **Низкую связанность** - минимальные зависимости между контроллерами
- **Простоте тестирования** - изолированная бизнес-логика

### Пример структуры:
```
Controllers/
├── Books/
│   ├── CreateBookController.cs
│   ├── ShowBookController.cs
│   ├── UpdateBookController.cs
│   ├── DeleteBookController.cs
│   └── ListBooksController.cs
└── Authors/
    └── ListAuthorsController.cs
```

### Пример контроллера:
```csharp
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
    /// <param name="book">Данные книги</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная книга</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExecuteAsync(
        [FromBody] Book book,
        CancellationToken cancellationToken = default)
    {
        var createdBook = await bookService.CreateBookAsync(book, cancellationToken);

        return Created(createdBook);
    }
}

```

## 🔄 9. Middleware система

### Архитектура middleware:
```
Middleware/
├── ExceptionHandlingMiddleware.cs
├── RequestLoggingMiddleware.cs
├── RouteLoggingMiddleware.cs
└── ValidationMiddleware.cs
```

### Порядок выполнения:
```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RouteLoggingMiddleware>();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
```

### Ключевые middleware:
1. **`ExceptionHandlingMiddleware`** - централизованная обработка исключений
2. **`RequestLoggingMiddleware`** - логирование входящих запросов
3. **`RouteNotFoundMiddleware`** - реакция на несуществующий маршрут

### Инициализация:
```csharp
public class MiddlewareSteps
{
    [BootstrapStep(70, "Configure middleware pipeline")]
    public static void ConfigureMiddleware(WebApplication app)
    {
        app.UseStaticFiles();
        
        // Логирование запросов
        app.UseMiddleware<RequestLoggingMiddleware>();
        
        // Обработка несуществующих маршрутов
        app.UseMiddleware<RouteNotFoundMiddleware>();
        
        // Обработка исключений
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
```

## 🏗️ 10. Общая архитектура проекта

### Принципы проектирования:
- **Clean Architecture** - разделение на слои
- **SOLID** - принципы объектно-ориентированного дизайна
- **DRY** - избегание дублирования кода
- **KISS** - простота реализации

### Преимущества архитектуры:
1. **Тестируемость** - каждый слой можно тестировать изолированно
2. **Поддерживаемость** - четкое разделение ответственности
3. **Масштабируемость** - возможность замены компонентов
4. **Гибкость** - легкая адаптация к изменениям требований

## 🎯 Заключение

Данная архитектура предоставляет:
- **Гибкую систему инициализации** через Bootstrap шаги
- **Надежную обработку ошибок** с кастомизируемыми метаданными
- **Единый формат ответов** для всех конечных точек API
- **Мощную систему валидации** с FluentValidation
- **Гибкую фильтрацию** через две системы спецификаций
- **Эффективное хранение данных** в памяти с поддержкой спецификаций
- **Полную документацию API** через Swagger
- **Чистую архитектуру** с Single Action Controllers
- **Модульную систему middleware** для cross-cutting concerns

Проект готов к переходу на базу данных (Entity Framework Core) и масштабированию функциональности.
