using BookLibrary.Contracts;
using BookLibrary.Dto;
using BookLibrary.Dto.Requests;
using BookLibrary.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/v1/books")]
public class BookController(
    IBookService bookService,
    ILogger<BookController> logger
) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BookResponse>>>> GetBooks(
        string? author = null,
        string? sortBy = null)
    {
        logger.LogInformation("Fetching books. Filters - Author: {Author}, SortBy: {SortBy}",
            author ?? "not specified", sortBy ?? "not specified");

        var books = await bookService.GetAllAsync(author, sortBy);

        logger.LogInformation("Successfully returned {BookCount} books.", books.Count);

        return Ok(new ApiResponse<List<BookResponse>>
        {
            Success = true,
            Data = books.ToResponseList(),
            Message = "Books retrieved successfully."
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BookResponse>>> GetBookById(int id)
    {
        logger.LogInformation("Fetching book by ID: {BookId}", id);

        var book = await bookService.GetByIdAsync(id);
        if (book == null)
        {
            logger.LogWarning("Book with ID {BookId} not found.", id);
            return NotFound(new ApiResponse
            {
                Success = false,
                Data = new { path = $"/books/{id}", timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                Message = $"Book with ID {id} not found."
            });
        }

        logger.LogInformation("Book with ID {BookId} retrieved successfully.", id);
        return Ok(new ApiResponse<BookResponse>
        {
            Success = true,
            Data = book.ToResponse(),
            Message = "Book retrieved successfully."
        });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookResponse>>> CreateBook(BookRequest request)
    {
        logger.LogInformation("Attempting to create a new book. Title: {Title}, Author: {Author}",
            request.Title, request.Author);

        try
        {
            var newBook = await bookService.CreateAsync(request.ToBook());
            logger.LogInformation("Book created successfully with ID {BookId}.", newBook.Id);

            return CreatedAtAction(
                nameof(GetBookById),
                new { id = newBook.Id },
                new ApiResponse<BookResponse>
                {
                    Success = true,
                    Data = newBook.ToResponse(),
                    Message = "Book created successfully."
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating book. Title: {Title}", request.Title);
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<BookResponse>>> UpdateBook(int id, BookRequest request)
    {
        logger.LogInformation("Attempting to update book with ID {BookId}. New title: {Title}", id, request.Title);

        var book = request.ToBook(id);
        bool success = await bookService.UpdateAsync(book);
        if (!success)
        {
            logger.LogWarning("Update failed: book with ID {BookId} not found.", id);

            return NotFound(new ApiResponse
            {
                Success = false,
                Data = new { path = $"/books/{id}", timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                Message = $"Book with ID {id} not found."
            });
        }

        logger.LogInformation("Book with ID {BookId} updated successfully.", id);

        return Ok(new ApiResponse<BookResponse>
        {
            Success = true,
            Data = book.ToResponse(),
            Message = "Book updated successfully."
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<BookResponse>>> DeleteBook(int id)
    {
        logger.LogInformation("Attempting to delete book with ID {BookId}", id);

        var success = await bookService.DeleteAsync(id);
        if (!success)
        {
            logger.LogWarning("Delete failed: book with ID {BookId} not found.", id);

            return NotFound(new ApiResponse
            {
                Success = false,
                Data = new { path = $"/books/{id}", timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                Message = $"Book with ID {id} not found."
            });
        }

        logger.LogInformation("Book with ID {BookId} deleted successfully.", id);

        return Ok(new ApiResponse<BookResponse>
        {
            Success = true,
            Message = "Book delete successfully."
        });
    }
}