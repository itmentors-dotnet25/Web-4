using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Dto.Requests;

public record BookRequest(
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(250, MinimumLength = 1, ErrorMessage = "Title length must be between 1 and 250 characters.")]
    string Title,

    [Required(ErrorMessage = "Author is required.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Author length must be between 2 and 150 characters.")]
    string Author,

    [RegularExpression(@"^(978|979)-\d{2}-\d{4}-\d{3}-\d$",
        ErrorMessage = "ISBN must follow the ISBN-13 XXX-XX-XXXX-XXX-X format (start with 978 or 979).")]
    string? Isbn,

    int PublicationYear,

    [StringLength(150, MinimumLength = 2, ErrorMessage = "Genre length must be between 2 and 150 characters.")]
    string? Genre,

    bool IsAvailable
);
