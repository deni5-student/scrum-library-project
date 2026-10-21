using SchoolLibrary.Models;
using SchoolLibrary.Repositories;

namespace SchoolLibrary.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return null;
            }

            return user;
        }
    }
}
