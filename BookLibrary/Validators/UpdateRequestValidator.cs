using BookLibrary.Contracts;

namespace BookLibrary.Validators;

public abstract class UpdateRequestValidator()
    : RequestValidatorBase<UpdateBookRequest>(x => x.Title, x => x.Author, x => x.ISBN, x => x.PublicationYear);