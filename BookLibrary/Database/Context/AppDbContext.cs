using BookLibrary.Database.Seeders;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Database.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DbSet для моделей
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка сущности Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            
            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("Isbn");
            
            entity.Property(b => b.PublicationYear)
                .IsRequired();
            
            entity.Property(b => b.Genre)
                .HasMaxLength(50);
            
            entity.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(b => b.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            // Внешние ключи
            entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Индексы
            entity.HasIndex(b => b.ISBN).IsUnique();
            entity.HasIndex(b => b.Title);
            entity.HasIndex(b => b.AuthorId);
            entity.HasIndex(b => b.CategoryId);
        });

        // Настройка сущности Author
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(a => a.Country)
                .HasMaxLength(50);
            
            entity.Property(a => a.Biography)
                .HasMaxLength(1000);
            
            entity.Property(a => a.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(a => a.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasIndex(a => a.Name);
        });

        // Настройка сущности Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(c => c.Description)
                .HasMaxLength(500);
            
            entity.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            // Уникальный индекс для имени категории
            entity.HasIndex(c => c.Name).IsUnique();
        });
        
        // Применяем все сидеры
        SeederFactory.ApplySeeders(modelBuilder);
    }
}
