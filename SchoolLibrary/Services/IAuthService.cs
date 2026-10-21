using SchoolLibrary.Models;

namespace SchoolLibrary.Services
{
    public interface IAuthService
    {
        User? Authenticate(string username, string password);
    }
}
