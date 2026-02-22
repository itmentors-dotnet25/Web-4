using BookLibrary.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Заголовок и версия API
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Books API",
        Version = "v1",
        Description = "Простой API для получения списка кник",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Stepan",
            Email = "stepanraikevich@gmail.com"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === Инициализация данных ===
List<Book> books = new List<Book>
{
    new Book { Id = 1, Title = "1984", Author = "Джордж Оруэлл", ISBN = "9780451524935", PublicationYear = 1949, Genre = "Антиутопия", IsAvailable = true },
    new Book { Id = 2, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", ISBN = "9785171017521", PublicationYear = 1967, Genre = "Фантастика", IsAvailable = false },
    new Book { Id = 3, Title = "Война и мир", Author = "Лев Толстой", ISBN = "9785389064297", PublicationYear = 1869, Genre = "Классика", IsAvailable = true },
    new Book { Id = 4, Title = "Гарри Поттер и философский камень", Author = "Дж. К. Роулинг", ISBN = "9780747532699", PublicationYear = 1997, Genre = "Фэнтези", IsAvailable = true },
    new Book { Id = 5, Title = "Преступление и наказание", Author = "Фёдор Достоевский", ISBN = "9785171017538", PublicationYear = 1866, Genre = "Классика", IsAvailable = false }
};

// === Глобальный логгер приложения ===
var logger = app.Logger;

// Логируем запуск приложения
logger.LogInformation("Book Library API запущен. Доступно книг: {BookCount}", books.Count);

app.MapGet("/books", () => {
    logger.LogDebug("Запрос списка всех книг");
    logger.LogInformation("Возвращено {BookCount} книг", books.Count);
    return Results.Ok(books);
})
    .WithName("GetAllBooks")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Получить список всех книг";
        operation.Description = "Возвращает полный список книг в библиотеке";
        operation.Responses["200"].Description = "Список книг успешно получен";
        return operation;
    })
    .Produces<List<Book>>(StatusCodes.Status200OK);

app.Run();