using BookLibrary.Config;

namespace BookLibrary.Bootstrap.BuilderSteps;

public class ConfigurationSteps
{
    [BootstrapStep(10, "Configure application settings")]
    public static void ConfigureSettings(WebApplicationBuilder builder)
    {
        // Настройка конфигурации
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        
        // Регистрация настроек приложения
        builder.Services.Configure<ApplicationSettings>(
            builder.Configuration.GetSection("ApplicationSettings"));
        
        // Дополнительные настройки...
    }
}
