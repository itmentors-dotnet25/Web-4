using BookLibrary.Database.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Validators.Rules;

public class AuthorExistsRule : AbstractValidator<int>
{
    public AuthorExistsRule(IServiceProvider serviceProvider)
    {
        RuleFor(id => id)
            .MustAsync(async (authorId, cancellation) =>
            {
                // Получаем контекст только если он зарегистрирован
                var context = serviceProvider.GetService(typeof(AppDbContext));
                
                if (context == null)
                {
                    // InMemory режим — пропускаем проверку существования
                    return true;
                }
                
                var dbContext = (AppDbContext)context;
                
                return await dbContext.Authors
                    .AnyAsync(a => a.Id == authorId, cancellation);
            })
            .WithMessage("Автор с указанным ID не найден");
    }
}
