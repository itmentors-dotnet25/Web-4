using BookLibrary.Config;
using BookLibrary.Contracts.Repositories;
using BookLibrary.Database.Context;
using BookLibrary.Repositories.AuthorRepositories.EFCore;
using BookLibrary.Repositories.AuthorRepositories.Memory;
using BookLibrary.Repositories.BookRepositories.EFCore;
using BookLibrary.Repositories.BookRepositories.Memory;
using BookLibrary.Storage;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Bootstrap.BuilderSteps;

public class DatabaseSteps
{
    [BootstrapStep(30, "Configure database context")]
    public static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        var storageSettings = builder.Configuration
            .GetSection("ApplicationSettings:Storage")
            .Get<StorageSettings>() ?? new StorageSettings();

        Console.WriteLine($"🗄️  Storage Provider: {storageSettings.Provider}");

        if (storageSettings.UseInMemory)
        {
            ConfigureInMemoryStorage(builder);
        }
        else if (storageSettings.UsePostgreSQL)
        {
            ConfigurePostgresStorage(builder);
        }
        else
        {
            throw new InvalidOperationException(
                $"Unsupported storage provider: {storageSettings.Provider}. " +
                "Supported providers: 'InMemory', 'PostgreSQL'");
        }
    }
    
    [BootstrapStep(31, "Apply database migrations (PostgreSQL only)")]
    public static void ApplyMigrations(WebApplicationBuilder builder)
    {
        var storageSettings = builder.Configuration
            .GetSection("ApplicationSettings:Storage")
            .Get<StorageSettings>() ?? new StorageSettings();

        Console.WriteLine(storageSettings.UsePostgreSQL
            ? "   → Database migrations will be applied on startup"
            : "   → Skipping database migrations (In-Memory storage)");
    }

    private static string MaskConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return string.Empty;

        var parts = connectionString.Split(';');
        var maskedParts = parts.Select(part =>
        {
            if (part.StartsWith("Password=", StringComparison.OrdinalIgnoreCase) ||
                part.StartsWith("Pwd=", StringComparison.OrdinalIgnoreCase))
            {
                return "Password=***";
            }
            return part;
        });

        return string.Join("; ", maskedParts);
    }
    
    private static void ConfigureInMemoryStorage(WebApplicationBuilder builder)
    {
        // ЕДИНСТВЕННЫЙ экземпляр хранилища на всё приложение
        builder.Services.AddSingleton<InMemoryStore>();
        
        // РЕГИСТРИРУЕМ РЕПОЗИТОРИИ ДЛЯ IN-MEMORY
        builder.Services.AddScoped<IBookRepository, MemoryBookRepository>();
        builder.Services.AddScoped<IAuthorReadRepository, MemoryAuthorReadRepository>();
    }
    
    private static void ConfigurePostgresStorage(WebApplicationBuilder builder)
    {
        Console.WriteLine("   → Using PostgreSQL storage");
        
        var connectionString = builder.Configuration
            .GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in configuration.");
        }

        Console.WriteLine($"   → Connection string: {MaskConnectionString(connectionString)}");

        // Регистрируем контекст БД
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                npgsqlOptions.CommandTimeout(60);
            });
            
            #if DEBUG
            options.EnableSensitiveDataLogging();
            options.LogTo(Console.WriteLine, LogLevel.Information);
            #endif
        });

        // Регистрируем репозитории для PostgreSQL
        builder.Services.AddScoped<IBookRepository, EfCoreBookRepository>();
        builder.Services.AddScoped<IAuthorReadRepository, EfCoreAuthorReadRepository>();
    }
}
