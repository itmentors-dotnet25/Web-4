using BookLibrary.Models;

namespace BookLibrary.Services;

public class BookService : IBookService
{
    private readonly ILogger<BookService> _logger;
    private readonly List<Book> _books;

    public BookService(ILogger<BookService> logger)
    {
        _logger = logger;

        _books = new List<Book>
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

        _logger.LogInformation("BookService инициализирован с {Count} книгами", _books.Count);
    }

    public Task<IEnumerable<Book>> GetAllBooksAsync(string? author = null, string? sortBy = null)
    {
        _logger.LogInformation("Получение всех книг. Фильтр: author={Author}, sort={SortBy}", author, sortBy);

        IEnumerable<Book> result = _books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(author))
        {
            result = result.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

            _logger.LogInformation("Фильтрация по автору '{Author}'. Найдено: {Count}", author, result.Count());
        }

        if (sortBy?.ToLower() == "title")
        {
            result = result.OrderBy(b => b.Title);

            _logger.LogInformation("Сортировка по названию");
        }

        return Task.FromResult(result);
    }

    public Task<Book?> GetBookByIdAsync(int id)
    {
        _logger.LogInformation("Получение книги с ID: {BookId}", id);
        
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            _logger.LogWarning("Книга с ID: {BookId} не найдена", id);
        }

        return Task.FromResult(book);
    }

    public Task<Book> CreateBookAsync(Book book)
    {
        _logger.LogInformation("Создание новой книги: {Title}", book.Title);
        
        book.Id = _books.Max(b => b.Id) + 1;
        
        _books.Add(book);
        _logger.LogInformation("Книга создана с ID: {BookId}", book.Id);
        
        return Task.FromResult(book);
    }

    public Task<Book?> UpdateBookAsync(int id, Book book)
    {
        _logger.LogInformation("Обновление книги с ID: {BookId}", id);
        
        var existingBook = _books.FirstOrDefault(b => b.Id == id);
        
        if (existingBook == null)
        {
            _logger.LogWarning("Книга с ID: {BookId} не найдена для обновления", id);
            return Task.FromResult<Book?>(null);
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.ISBN = book.ISBN;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.Genre = book.Genre;
        existingBook.IsAvailable = book.IsAvailable;

        _logger.LogInformation("Книга с ID: {BookId} успешно обновлена", id);
        
        return Task.FromResult<Book?>(existingBook);
    }

    public Task<bool> DeleteBookAsync(int id)
    {
        _logger.LogInformation("Удаление книги с ID: {BookId}", id);
        
        var book = _books.FirstOrDefault(b => b.Id == id);
        
        if (book == null)
        {
            _logger.LogWarning("Книга с ID: {BookId} не найдена для удаления", id);
            
            return Task.FromResult(false);
        }

        _books.Remove(book);
        
        _logger.LogInformation("Книга с ID: {BookId} успешно удалена", id);
        
        return Task.FromResult(true);
    }
}