using BookLibrary.Models;
using BookLibrary.Validators.Rules;
using FluentValidation;

namespace BookLibrary.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название книги обязательно для заполнения")
            .MinimumLength(1).WithMessage("Название должно быть от 1 до 255 символов")
            .MaximumLength(255).WithMessage("Название должно быть от 1 до 255 символов")
            .WithName("title");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Автор книги обязателен для заполнения")
            .MinimumLength(2).WithMessage("Автор должен быть от 2 до 100 символов")
            .MaximumLength(100).WithMessage("Автор должен быть от 2 до 100 символов")
            .WithName("author");

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
