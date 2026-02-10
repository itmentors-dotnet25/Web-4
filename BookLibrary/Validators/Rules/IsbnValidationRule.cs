using FluentValidation;

namespace BookLibrary.Validators.Rules;

public static class IsbnValidationRule
{
    public static IRuleBuilderOptions<T, string> ValidIsbn<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("ISBN обязателен для заполнения")
            .Matches(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d$")
            .WithMessage("ISBN должен быть в формате XXX-XX-XXXX-XXX-X (например: 978-56-9912-345-6)")
            .Length(17);
    }
}
