using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Validators;

/// <summary>
/// Валидация года в диапазоне от минимального до текущего года
/// </summary>
public class YearRangeAttribute : ValidationAttribute
{
    private readonly int _minYear;
    private readonly bool _allowFutureYears;

    /// <summary>
    /// Валидация года в диапазоне от minYear до текущего года (или будущего, если разрешено)
    /// </summary>
    /// <param name="minYear">Минимальный год</param>
    /// <param name="allowFutureYears">Разрешить годы в будущем (по умолчанию false)</param>
    public YearRangeAttribute(int minYear, bool allowFutureYears = false)
    {
        _minYear = minYear;
        _allowFutureYears = allowFutureYears;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success; // null разрешен для nullable полей

        if (value is not int year)
            return new ValidationResult("Значение должно быть целым числом.");

        var maxYear = _allowFutureYears ? DateTime.Now.Year + 10 : DateTime.Now.Year;
        var currentYear = DateTime.Now.Year;

        if (year < _minYear)
        {
            return new ValidationResult(
                $"Год должен быть не менее {_minYear}.");
        }

        if (!_allowFutureYears && year > currentYear)
        {
            return new ValidationResult(
                $"Год не может быть больше текущего года ({currentYear}).");
        }

        if (_allowFutureYears && year > maxYear)
        {
            return new ValidationResult(
                $"Год не может быть больше {maxYear}.");
        }

        return ValidationResult.Success;
    }
}
