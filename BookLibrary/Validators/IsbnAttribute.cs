using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookLibrary.Validators;

public partial class IsbnAttribute : ValidationAttribute
{
    private static readonly Regex IsbnRegex =
        MyRegex();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || value is string isbn && IsbnRegex.IsMatch(isbn))
            return ValidationResult.Success;

        return new ValidationResult(
            "ISBN must follow the ISBN-13 XXX-XX-XXXX-XXX-X format (start with 978 or 979).");
    }

    [GeneratedRegex(@"^(978|979)-\d{2}-\d{4}-\d{3}-\d$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
