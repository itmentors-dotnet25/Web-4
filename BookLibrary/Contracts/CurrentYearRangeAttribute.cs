using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Contracts;

public sealed class CurrentYearRangeAttribute(int min) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is not int year)
        {
            return false;
        }

        return year >= min && year <= DateTime.UtcNow.Year;
    }
}