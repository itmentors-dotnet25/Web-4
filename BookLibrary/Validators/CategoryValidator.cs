using BookLibrary.Models;
using FluentValidation;

namespace BookLibrary.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название категории обязательно для заполнения")
            .MinimumLength(2).WithMessage("Название должно быть от 2 до 100 символов")
            .MaximumLength(100).WithMessage("Название должно быть от 2 до 100 символов")
            .WithName("name");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов")
            .WithName("description");
    }
}
