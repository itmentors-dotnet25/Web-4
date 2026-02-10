using BookLibrary.Storage;

namespace BookLibrary.Bootstrap.BuilderSteps;

public class DatabaseSteps
{
    [BootstrapStep(30, "Configure database context")]
    public static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        // builder.Services.AddDbContext<BookLibraryDbContext>(options =>
        // {
        //     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        // });
    }
    
    [BootstrapStep(31, "Configure InMemory storage")]
    public static void ConfigureInMemoryStorage(WebApplicationBuilder builder)
    {
        // ЕДИНСТВЕННЫЙ экземпляр хранилища на всё приложение
        builder.Services.AddSingleton<InMemoryStore>();
    }
}
