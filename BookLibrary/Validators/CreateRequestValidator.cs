using BookLibrary.Contracts;

namespace BookLibrary.Validators;

public abstract class CreateRequestValidator()
    : RequestValidatorBase<CreateBookRequest>(x => x.Title, x => x.Author, x => x.ISBN, x => x.PublicationYear);