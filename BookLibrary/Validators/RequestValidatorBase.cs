using System.Linq.Expressions;
using FluentValidation;

namespace BookLibrary.Validators;

public abstract class RequestValidatorBase<T> : AbstractValidator<T>
{
    protected RequestValidatorBase(
        Expression<Func<T, string>> title,
        Expression<Func<T, string>> author,
        Expression<Func<T, string>> isbn,
        Expression<Func<T, int>> year)
    {
        RuleFor(title)
            .NotEmpty().WithMessage("Название книги обязательно для заполнения")
            .MaximumLength(200).WithMessage("Название должно содержать не более 200 символов");

        RuleFor(author)
            .NotEmpty().WithMessage("Автор обязательно для заполнения")
            .MaximumLength(200).WithMessage("Имя автора должно содержать не более 200 символов");

        RuleFor(isbn)
            .NotEmpty().WithMessage("ISBN обязательно для заполнения")
            .Matches(@"^\d{3}-\d{10}$")
            .WithMessage("ISBN должен быть в формате XXX-XXXXXXXXXX");

        RuleFor(year)
            .GreaterThanOrEqualTo(1000).WithMessage("Год издания не может быть меньше 1000")
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Год издания не может превышать текущий год");
    }
}