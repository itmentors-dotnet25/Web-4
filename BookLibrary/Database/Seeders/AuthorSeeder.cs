using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Database.Seeders;

public class AuthorSeeder : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "Агата Кристи"},
            new Author { Id = 2, Name = "Артур Конан Дойл"},
            new Author { Id = 3, Name = "Джоан Роулинг"},
            new Author { Id = 4, Name = "Фёдор Достоевский"},
            new Author { Id = 5, Name = "Лев Толстой"}
        );
    }
}
