using BookLibrary.Models;
using BookLibrary.Validators;
using FluentValidation;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.ISBN)
            .SetValidator(new IsbnValidator());
    }
}