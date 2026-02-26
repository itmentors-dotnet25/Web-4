using BookLibrary.Models;

namespace BookLibrary.Services;

public class BookService : IBookService
{
    private static readonly object Sync = new();

    private static readonly List<Book> Books = new()
    {
        new Book
        {
            Id = 1, Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", ISBN = "978-0544003415",
            PublicationYear = 1954, Genre = "Fantasy", IsAvailable = true
        },
        new Book
        {
            Id = 2, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", PublicationYear = 1949,
            Genre = "Dystopian", IsAvailable = true
        },
        new Book
        {
            Id = 3, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0141439518",
            PublicationYear = 1813, Genre = "Romance", IsAvailable = false
        }
    };

    public BookService(ILogger<BookService> logger)
    {
        logger.LogInformation("BookService initialized. Count={Count}", Books.Count);
    }

    public Task<IEnumerable<Book>> GetAllBooksAsync(string? author = null, string? sortBy = null)
    {
        IEnumerable<Book> result;
        lock (Sync)
        {
            result = Books.ToList();
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            result = result.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        if (string.Equals(sortBy, "title", StringComparison.OrdinalIgnoreCase))
        {
            result = result.OrderBy(b => b.Title);
        }

        return Task.FromResult(result);
    }

    public Task<Book?> GetBookByIdAsync(int id)
    {
        Book? book;

        lock (Sync)
        {
            book = Books.FirstOrDefault(b => b.Id == id);
        }

        return Task.FromResult(book);
    }

    public Task<Book> CreateBookAsync(Book book)
    {
        lock (Sync)
        {
            if (Books.Any(b => string.Equals(b.ISBN, book.ISBN, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"ISBN '{book.ISBN}' already exists");
            }

            var nextId = Books.Count == 0 ? 1 : Books.Max(b => b.Id) + 1;
            book.Id = nextId;

            Books.Add(book);
        }

        return Task.FromResult(book);
    }
    public Task<Book?> UpdateBookAsync(int id, Book book)
    {
        Book? existing;

        lock (Sync)
        {
            existing = Books.FirstOrDefault(b => b.Id == id);
            if (existing == null) return Task.FromResult<Book?>(null);

            if (Books.Any(b => b.Id != id && string.Equals(b.ISBN, book.ISBN, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"ISBN '{book.ISBN}' already exists");

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.ISBN = book.ISBN;
            existing.PublicationYear = book.PublicationYear;
            existing.Genre = book.Genre;
            existing.IsAvailable = book.IsAvailable;
        }

        return Task.FromResult<Book?>(existing);
    }

    public Task<bool> DeleteBookAsync(int id)
    {
        lock (Sync)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return Task.FromResult(false);
            }

            Books.Remove(book);

            return Task.FromResult(true);
        }
    }

    internal static void ResetForTests()
    {
        lock (Sync)
        {
            Books.Clear();
            Books.AddRange([
                new Book
                {
                    Id = 1, Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", ISBN = "978-0544003415",
                    PublicationYear = 1954, Genre = "Fantasy", IsAvailable = true
                },
                new Book
                {
                    Id = 2, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", PublicationYear = 1949,
                    Genre = "Dystopian", IsAvailable = true
                },
                new Book
                {
                    Id = 3, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0141439518",
                    PublicationYear = 1813, Genre = "Romance", IsAvailable = false
                }
            ]);
        }
    }
}