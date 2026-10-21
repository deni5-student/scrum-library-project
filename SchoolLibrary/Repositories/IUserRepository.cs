using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
    }
}
