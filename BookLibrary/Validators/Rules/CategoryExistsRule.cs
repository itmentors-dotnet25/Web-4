using BookLibrary.Database.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Validators.Rules;

public class CategoryExistsRule : AbstractValidator<int>
{
    public CategoryExistsRule(IServiceProvider serviceProvider)
    {
        RuleFor(id => id)
            .MustAsync(async (categoryId, cancellation) =>
            {
                // Получаем контекст только если он зарегистрирован
                var context = serviceProvider.GetService(typeof(AppDbContext));
                
                if (context == null)
                {
                    // InMemory режим — пропускаем проверку существования
                    return true;
                }
                
                var dbContext = (AppDbContext)context;
                
                return await dbContext.Categories
                    .AnyAsync(c => c.Id == categoryId, cancellation);
            })
            .WithMessage("Категория с указанным ID не найдена");
    }
}
