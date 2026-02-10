using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Tests.IntegrationTests;

public class BookLibraryWebApplicationFactory : WebApplicationFactory<Program>
{
    public BookLibraryWebApplicationFactory()
    {
        // Устанавливаем переменную окружения ДО выполнения Program.Main()
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Здесь можно заменить реальные сервисы на Mock.
            // Поскольку, и так работаем с памятью - то ничего не нужно
            // Но можно было бы заменить реальные сервисы на тестовые
            // services.AddSingleton<InMemoryStore>();
            // services.AddScoped<IBookRepository, MemoryBookRepository>();
        });
        
        // Настроить уровень логирования только для тестов
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Critical); // Только критические ошибки
        });
    }
}
