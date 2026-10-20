using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
    }
}