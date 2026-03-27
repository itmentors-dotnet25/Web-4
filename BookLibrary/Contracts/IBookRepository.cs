using BookLibrary.Models;

namespace BookLibrary.Contracts;

public interface IBookRepository
{
    IReadOnlyList<Book> GetAll();
    Book? GetById(int id);
    void Add(Book book);
    bool Update(Book book);
    bool Delete(int id);
}
