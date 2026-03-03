using BookLibrary.Requests;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BookLibrary.Validators;

public class IsbnValidator : AbstractValidator<string>
{
    public IsbnValidator()
    {
        RuleFor(ISBN => ISBN)
            .NotEmpty().WithMessage("ISBN не может быть пустым")
            .Must(IsValidIsbnFormat).WithMessage("Неверный формат ISBN. Ожидается: XXX-XX-XXXX-XXX-X");
    }

    /// <summary>
    /// Проверка формата: 3-2-4-3-1 цифра (или 'X' в конце)
    /// Пример: 978-0-306-40615-7
    /// </summary>
    private bool IsValidIsbnFormat(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return false;

        // Регулярное выражение для формата XXX-XX-XXXX-XXX-X
        var pattern = @"^\d{3}-\d{2}-\d{4}-\d{3}-[\dX]$";
        return Regex.IsMatch(isbn, pattern);
    }
}