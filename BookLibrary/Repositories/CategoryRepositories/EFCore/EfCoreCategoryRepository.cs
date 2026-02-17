using BookLibrary.Contracts.Repositories;
using BookLibrary.Data.Responses.Categories;
using BookLibrary.Database.Context;
using BookLibrary.Exceptions.Base;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories.CategoryRepositories.EFCore;

public class EfCoreCategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .Include(c => c.Books)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .Include(c => c.Books)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category == null)
            throw new ModelNotFoundException($"Category with ID {id} not found");

        return category;
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;
        
        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);
        
        return category;
    }

    public async Task<Category> UpdateAsync(int id, Category category, CancellationToken cancellationToken = default)
    {
        var existingCategory = await GetByIdAsync(id, cancellationToken);
        
        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.UpdatedAt = DateTime.UtcNow;
        
        context.Categories.Update(existingCategory);
        await context.SaveChangesAsync(cancellationToken);
        
        return existingCategory;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category == null)
            return false;

        // Проверяем, есть ли книги в этой категории
        var hasBooks = await context.Books
            .AnyAsync(b => b.CategoryId == id, cancellationToken);

        if (hasBooks)
        {
            throw new InvalidOperationException(
                $"Cannot delete category with ID {id} because it has associated books");
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
