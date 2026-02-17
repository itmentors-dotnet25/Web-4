using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Database.Seeders;

public class CategorySeeder : IDataSeeder
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
