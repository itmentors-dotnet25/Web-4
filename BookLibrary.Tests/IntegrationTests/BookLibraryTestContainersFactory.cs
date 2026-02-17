using System.Text;
using BookLibrary.Database.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql; // Работает ТОЛЬКО после установки Testcontainers.PostgreSql

namespace BookLibrary.Tests.IntegrationTests;

public class BookLibraryTestContainersFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:15-alpine")
        .WithDatabase("booklibrary_integration_test")
        .WithUsername("testuser")
        .WithPassword("SecureTestPass123!")
        .WithCleanUp(true)
        .Build();

    // Для отладки: публичное свойство строки подключения
    public string TestConnectionString => _dbContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        // 🔤 Исправляем кодировку КОНСОЛИ
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        
        // 🔇 Отключаем логи Testcontainers
        Environment.SetEnvironmentVariable("TESTCONTAINERS_LOGGING_DISABLED", "true");
        
        // 🔒 КРИТИЧЕСКАЯ ПРОВЕРКА: Защита от случайного подключения к боевой БД
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == "production")
        {
            throw new InvalidOperationException(
                "❌ ЗАПРЕЩЕНО запускать интеграционные тесты в окружении Production! " +
                "Установите переменную окружения ASPNETCORE_ENVIRONMENT=Test");
        }

        await _dbContainer.StartAsync();
        
        Console.WriteLine($"✅ Тестовый контейнер запущен. ConnectionString: {TestConnectionString}");
        
        // Применяем миграции ТОЛЬКО к тестовой БД в контейнере
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Дополнительная проверка: убеждаемся, что подключены к контейнеру
        var actualConnection = dbContext.Database.GetDbConnection().ConnectionString;
        if (!actualConnection.Contains("booklibrary_integration_test"))
        {
            throw new InvalidOperationException(
                $"❌ КРИТИЧЕСКАЯ ОШИБКА: Приложение подключено к НЕПРАВИЛЬНОЙ БД!\n" +
                $"Подключено к: {actualConnection}\n" +
                $"Ожидалось подключение к: {TestConnectionString}");
        }
        
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("✅ Миграции применены к тестовой БД");
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
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        
        // 🔒 ШАГ 2: Переопределяем строку подключения ЧЕРЕЗ ПЕРЕМЕННУЮ ОКРУЖЕНИЯ (высший приоритет!)
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _dbContainer.GetConnectionString());
        
        // 🔒 ШАГ 3: Дополнительно переопределяем через конфигурацию
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString()
            }!);
        });
        
        // 🔒 ШАГ 4: Заменяем контекст БД на тот, что использует строку из контейнера
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            
            if (descriptor != null)
                services.Remove(descriptor);
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
        
        // 🔒 ШАГ 5: 🔇 Отключаем логи приложения (только Critical)
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddFilter("Microsoft", LogLevel.Critical);
            logging.AddFilter("System", LogLevel.Critical);
            logging.AddFilter("Testcontainers", LogLevel.None);
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Critical);
        });
    }
}
