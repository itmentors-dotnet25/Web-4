using BookLibrary.Models;
using FluentValidation;

namespace BookLibrary.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название книги обязательно для заполнения")
            .MaximumLength(200).WithMessage("Название должно содержать не более 200 символов");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Автор обязательно для заполнения")
            .MaximumLength(200).WithMessage("Имя автора должно содержать не более 200 символов");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN обязательно для заполнения")
            .Matches(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d{1}$")
            .WithMessage("ISBN должен быть в формате XXX-XX-XXXX-XXX-X");

        RuleFor(x => x.PublicationYear)
            .GreaterThanOrEqualTo(1000).WithMessage("Год издания не может быть меньше 1000")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Год издания не может превышать текущий год");
    }
}