using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookLibrary.Contracts;

public sealed partial class IsbnAttribute : ValidationAttribute
{
    private static readonly Regex IsbnRegex = BookIsbnRegex();

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is not string isbn)
        {
            return false;
        }

        return IsbnRegex.IsMatch(isbn);
    }

    [GeneratedRegex(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d{1}$")]
    private static partial Regex BookIsbnRegex();
}