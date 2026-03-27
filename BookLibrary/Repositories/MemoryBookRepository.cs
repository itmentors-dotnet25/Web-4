using BookLibrary.Contracts;
using BookLibrary.Models;

namespace BookLibrary.Repositories;

public class MemoryBookRepository : IBookRepository
{
    private int _nextId = 1;

    private readonly List<Book> _books;

    public MemoryBookRepository()
    {
        _books =
        [
            new Book
            {
                Id = GetNextId(),
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Isbn = "978-11-1111-000-1",
                PublicationYear = 2008,
                Genre = "Technical literature",
                IsAvailable = true
            },

            new Book
            {
                Id = GetNextId(),
                Title = "Clean Architecture",
                Author = "Robert C. Martin [sort]",
                Isbn = "978-11-1111-000-2",
                PublicationYear = 2017,
                Genre = "Technical literature",
                IsAvailable = true
            },

            new Book
            {
                Id = GetNextId(),
                Title = "Clean Agile",
                Author = "Robert C. Martin",
                Isbn = "978-11-1111-000-3",
                PublicationYear = 2019,
                Genre = "Technical literature",
                IsAvailable = true
            }
        ];
    }

    public IReadOnlyList<Book> GetAll() => _books.AsReadOnly();

    public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public void Add(Book book)
    {
        book.Id = GetNextId();
        _books.Add(book);
    }

    public bool Update(Book book)
    {
        var index = _books.FindIndex(b => b.Id == book.Id);
        if (index == -1)
            return false;

        _books[index] = book;
        return true;
    }

    public bool Delete(int id)
    {
        var index = _books.FindIndex(b => b.Id == id);
        if (index == -1)
            return false;

        _books.RemoveAt(index);
        return true;
    }

    private int GetNextId() => _nextId++;
}
