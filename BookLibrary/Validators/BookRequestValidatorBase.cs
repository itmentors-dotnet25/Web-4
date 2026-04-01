using BookLibrary.Contracts;
using FluentValidation;

namespace BookLibrary.Validators;

public abstract class BookRequestValidatorBase<T> : AbstractValidator<T>
    where T : BookRequestBase
{
    protected BookRequestValidatorBase()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название книги обязательно для заполнения")
            .MaximumLength(200).WithMessage("Название должно содержать не более 200 символов");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN обязательно для заполнения")
            .Matches(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d{1}$")
            .WithMessage("ISBN должен быть в формате XXX-XX-XXXX-XXX-X");

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1000, DateTime.UtcNow.Year)
            .WithMessage("Год издания должен быть между 1000 и текущим годом");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("ID автора должен быть больше 0");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("ID категории должен быть больше 0");
    }
}

public sealed class CreateRequestValidator : BookRequestValidatorBase<CreateBookRequest>
{
}

public sealed class UpdateRequestValidator : BookRequestValidatorBase<UpdateBookRequest>
{
}