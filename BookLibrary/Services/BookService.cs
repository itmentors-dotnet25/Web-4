using BookLibrary.Models;
using BookLibrary.Requests;

namespace BookLibrary.Services;

/// <summary>
/// Сервис библиотеки
/// </summary>
public class BookService : IBookService
{
    private readonly List<Book> _books;
    private int _nextId;

    /// <summary>
    /// Статический список книг
    /// </summary>
    public BookService()
    {
        // Инициализируем данные в конструкторе
        _books =
            [
                new() { Id = 1, Title = "Война и мир", Author = "Лев Толстой", ISBN = "978-5-699-12345-6", PublicationYear = 1869},
                new() { Id = 2, Title = "Анна Каренина", Author = "Лев Толстой", ISBN = "978-5-699-23456-7", PublicationYear = 1877},
                new() { Id = 3, Title = "Преступление и наказание", Author = "Фёдор Достоевский", ISBN = "978-5-699-34567-8", PublicationYear = 1866},
                new() { Id = 4, Title = "Идиот", Author = "Фёдор Достоевский", ISBN = "978-5-699-45678-9", PublicationYear = 1869},
                new() { Id = 5, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", ISBN = "978-5-699-56789-0", PublicationYear = 1967},
                new() { Id = 6, Title = "Собачье сердце", Author = "Михаил Булгаков", ISBN = "978-5-699-67890-1", PublicationYear = 1925},
                new() { Id = 7, Title = "Отцы и дети", Author = "Иван Тургенев", ISBN = "978-5-699-78901-2", PublicationYear = 1862},
                new() { Id = 8, Title = "Евгений Онегин", Author = "Александр Пушкин", ISBN = "978-5-699-89012-3", PublicationYear = 1833}
            ];

        _nextId = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
    }


    /// <summary>
    /// Получить все киниги
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public List<Book> GetAll(BookFilterRequest filter)
    {
        var query = _books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter.Author))
        {
            var authorFilter = filter.Author.Trim().ToLower();
            query = query.Where(b =>
                b.Author != null &&
                b.Author.Contains(authorFilter, StringComparison.CurrentCultureIgnoreCase));
        }

        query = ApplySorting(query, filter.SortBy, filter.SortOrder);

        return [.. query];
    }

    /// <summary>
    /// Получить книгу по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Book? GetBookById(int id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    /// <summary>
    /// Создать книгу
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    public Book CreateBook(Book book)
    {
        var newBook = new Book
        {
            Id = _nextId++,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            PublicationYear = book.PublicationYear
        };

        _books.Add(newBook);
        return newBook;
    }

    /// <summary>
    /// Обновить книгу
    /// </summary>
    /// <param name="id"></param>
    /// <param name="book"></param>
    /// <returns></returns>
    public Book? UpdateBook(int id, Book? book)
    {
        ArgumentNullException.ThrowIfNull(book);

        var existingBook = _books.FirstOrDefault(b => b.Id == id);

        if (existingBook == null)
            return null;

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.ISBN = book.ISBN;
        existingBook.PublicationYear = book.PublicationYear;

        return existingBook;
    }

    /// <summary>
    /// Удалить книгу
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteBook(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        if (book == null)
            return false;

        _books.Remove(book);
        return true;
    }

    /// <summary>
    /// Сортировка списка
    /// </summary>
    /// <param name="query"></param>
    /// <param name="sortBy"></param>
    /// <param name="sortOrder"></param>
    /// <returns></returns>
    private static IEnumerable<Book> ApplySorting(IEnumerable<Book> query, string? sortBy, string? sortOrder)
    {
        var order = sortOrder?.Trim().ToLower() ?? "asc";
        var field = sortBy?.Trim().ToLower() ?? "title";

        return (field, order) switch
        {
            ("title", "asc") => query.OrderBy(b => b.Title, StringComparer.OrdinalIgnoreCase),
            ("title", "desc") => query.OrderByDescending(b => b.Title, StringComparer.OrdinalIgnoreCase),

            _ => query.OrderBy(b => b.Title, StringComparer.OrdinalIgnoreCase)
        };
    }
}