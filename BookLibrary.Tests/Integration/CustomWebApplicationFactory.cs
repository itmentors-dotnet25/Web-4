using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookLibrary.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private string _databaseName = $"BookLibraryTestsDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureCreated();
            SeedDatabase(db);
        });
    }

    public void ResetDatabase()
    {
        _databaseName = $"BookLibraryTestsDb_{Guid.NewGuid()}";

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        SeedDatabase(db);
    }

    private static void SeedDatabase(ApplicationDbContext db)
    {
        if (db.Authors.Any() || db.Categories.Any() || db.Books.Any())
        {
            return;
        }

        var authors = new List<Author>
        {
            new()
            {
                Id = 1,
                FirstName = "Александр",
                LastName = "Пушкин",
                Bio = "Русский поэт"
            },
            new()
            {
                Id = 2,
                FirstName = "Лев",
                LastName = "Толстой",
                Bio = "Русский писатель"
            }
        };

        var categories = new List<Category>
        {
            new()
            {
                Id = 1,
                Name = "Поэзия",
                Description = "Стихотворения и поэмы"
            },
            new()
            {
                Id = 2,
                Name = "Проза",
                Description = "Романы и повести"
            }
        };

        var books = new List<Book>
        {
            new()
            {
                Id = 1,
                Title = "Евгений Онегин",
                ISBN = "978-50-4123-456-7",
                PublicationYear = 1833,
                Genre = "Роман в стихах",
                IsAvailable = true,
                AuthorId = 1,
                CategoryId = 1
            },
            new()
            {
                Id = 2,
                Title = "Война и мир",
                ISBN = "978-50-4123-457-4",
                PublicationYear = 1869,
                Genre = "Роман",
                IsAvailable = true,
                AuthorId = 2,
                CategoryId = 2
            }
        };

        db.Authors.AddRange(authors);
        db.Categories.AddRange(categories);
        db.Books.AddRange(books);

        db.SaveChanges();
    }
}