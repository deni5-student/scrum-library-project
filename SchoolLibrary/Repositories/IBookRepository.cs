using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        Book? GetById(int id);
        void UpdateAvailable(int bookId, int newAvailable);
    }
}