using SchoolLibrary.Models;

namespace SchoolLibrary.Services
{
    public interface IAuthService
    {
        User? ValidateUser(string username, string password);
    }
}