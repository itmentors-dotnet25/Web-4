using BookLibrary.Models;
using BookLibrary.Requests;

namespace BookLibrary.Services;

public interface IBookService
{
    List<Book> GetAll(BookFilterRequest filter);
    Book? GetBookById(int id);
    Book CreateBook(Book book);
    Book? UpdateBook(int id, Book book);
    bool DeleteBook(int id);
}