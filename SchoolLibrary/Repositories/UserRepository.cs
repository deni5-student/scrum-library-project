using Microsoft.Data.Sqlite;
using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public User? GetByUsername(string username)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(
                "SELECT Id, Username, PasswordHash, Role FROM Users WHERE Username = @username",
                connection);
            command.Parameters.AddWithValue("@username", username);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }
    }
}