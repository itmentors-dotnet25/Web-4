using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Database.Seeders;

public class BookSeeder : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book 
            { 
                Id = 1, 
                Title = "Десять негритят", 
                AuthorId = 1, 
                CategoryId = 1,
                ISBN = "978-5-17-101234-5",
                PublicationYear = 1939,
                Genre = "Детектив",
                IsAvailable = true
            },
            new Book 
            { 
                Id = 2, 
                Title = "Убийство в Восточном экспрессе", 
                AuthorId = 1, 
                CategoryId = 1,
                ISBN = "978-5-17-101235-2",
                PublicationYear = 1934,
                Genre = "Детектив",
                IsAvailable = true
            },
            new Book 
            { 
                Id = 3, 
                Title = "Гарри Поттер и философский камень", 
                AuthorId = 3, 
                CategoryId = 2,
                ISBN = "978-5-17-112345-6",
                PublicationYear = 1997,
                Genre = "Фэнтези",
                IsAvailable = true
            },
            new Book 
            { 
                Id = 4, 
                Title = "Преступление и наказание", 
                AuthorId = 4, 
                CategoryId = 3,
                ISBN = "978-5-699-12345-6",
                PublicationYear = 1866,
                Genre = "Роман",
                IsAvailable = true
            },
            new Book 
            { 
                Id = 5, 
                Title = "Война и мир", 
                AuthorId = 5, 
                CategoryId = 5,
                ISBN = "978-5-699-23456-7",
                PublicationYear = 1869,
                Genre = "Роман",
                IsAvailable = true
            },
            // 6. Исправленная книга (меняем данные, но Id оставляем 6)
            new Book 
            { 
                Id = 6, 
                Title = "Шерлок Холмс: Сокращенное издание", // Исправлено название
                AuthorId = 2, 
                CategoryId = 1,
                ISBN = "978-5-699-34567-8",
                PublicationYear = 1902,
                Genre = "Детектив",
                IsAvailable = true
            },
            
            // 7. Новая книга (Новый Id!)
            new Book 
            { 
                Id = 7, 
                Title = "Идиот", 
                AuthorId = 4, 
                CategoryId = 5,
                ISBN = "978-5-17-999910-9",
                PublicationYear = 1949,
                Genre = "Роман",
                IsAvailable = true
            },
            
            // 8. Новая книга (Новый Id!)
            new Book 
            { 
                Id = 8, 
                Title = "Бесы", 
                AuthorId = 4, 
                CategoryId = 5,
                ISBN = "978-5-17-888810-8",
                PublicationYear = 1965,
                Genre = "Роман",
                IsAvailable = true
            }
        );
    }
}
