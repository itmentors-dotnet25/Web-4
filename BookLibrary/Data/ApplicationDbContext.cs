using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne(book => book.Author)
            .WithMany(author => author.Books)
            .HasForeignKey(book => book.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasOne(book => book.Category)
            .WithMany(category => category.Books)
            .HasForeignKey(book => book.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasIndex(book => book.ISBN)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(category => category.Name)
            .IsUnique();

        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "Александр", LastName = "Пушкин", Bio = "Русский поэт" },
            new Author { Id = 2, FirstName = "Лев", LastName = "Толстой", Bio = "Русский писатель" }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Поэзия", Description = "Стихотворения и поэмы" },
            new Category { Id = 2, Name = "Проза", Description = "Романы и повести" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
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
            new Book
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
        );

        base.OnModelCreating(modelBuilder);
    }
}