using BookLibrary.Data.Requests.Book;
using BookLibrary.Validators.Rules;
using FluentValidation;

namespace BookLibrary.Validators;

public class BookValidator : AbstractValidator<CreateBookRequest>
{
    public BookValidator(
        AuthorExistsRule authorExistsRule,
        CategoryExistsRule categoryExistsRule
        )
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название книги обязательно для заполнения")
            .MinimumLength(1).WithMessage("Название должно быть от 1 до 255 символов")
            .MaximumLength(255).WithMessage("Название должно быть от 1 до 255 символов")
            .WithName("title");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("Автор книги обязателен для заполнения")
            .SetValidator(authorExistsRule)
            .WithName("authorId");
        
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Категория книги обязательна для заполнения")
            .SetValidator(categoryExistsRule)
            .WithName("categoryId");

        RuleFor(x => x.ISBN)
            .ValidIsbn()
            .WithName("isbn");

        RuleFor(x => x.PublicationYear)
            .NotEmpty().WithMessage("Год публикации обязателен для заполнения")
            .InclusiveBetween(1000, DateTime.Now.Year).WithMessage($"Год публикации должен быть в диапазоне от 1000 до {DateTime.Now.Year}")
            .WithName("publicationYear");

        RuleFor(x => x.Genre)
            .MaximumLength(50).WithMessage("Жанр не должен превышать 50 символов")
            .WithName("genre");
    }
}
