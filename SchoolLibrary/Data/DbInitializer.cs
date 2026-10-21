using Microsoft.Data.Sqlite;
using SchoolLibrary.Services;

namespace SchoolLibrary.Data
    {
        public class DbInitializer
        {
            private readonly string SQL_connectionString; 
            private readonly IPasswordHasher _passwordHasher;

            
        

            public DbInitializer(IConfiguration configuration, IPasswordHasher passwordHasher)
            {
                SQL_connectionString = configuration.GetConnectionString("Default")
                    ?? throw new InvalidOperationException("Connection string 'Default' not found.");
                _passwordHasher = passwordHasher;
        }

            public void CreateTables()
            {
                using var connection = new SqliteConnection(SQL_connectionString);
                connection.Open();

                var query = @"
                CREATE TABLE IF NOT EXISTS Books (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Author TEXT NOT NULL,
                    Total INTEGER NOT NULL,
                    Available INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Salt TEXT NOT NULL,
                    Role TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Loans (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    BookId INTEGER NOT NULL,
                    Username TEXT NOT NULL,
                    BorrowedAt TEXT NOT NULL,
                    DueDate TEXT NOT NULL,
                    ReturnedAt TEXT NULL
                );";

                using var command = new SqliteCommand(query, connection);
                command.ExecuteNonQuery();

            SeedBooks();
            SeedUsers();
        }

        public void SeedBooks()
        {
            using var connection = new SqliteConnection(SQL_connectionString);
            connection.Open();


            using var countCommand = new SqliteCommand("SELECT COUNT(*) FROM Books", connection);
            long count = (long)countCommand.ExecuteScalar()!;

            if (count > 0)
            {
                return;
            }

            var insertQuery = @"
        INSERT INTO Books (Title, Author, Total, Available) VALUES
            ('The Hobbit', 'J.R.R. Tolkien', 5, 5),
            ('1984', 'George Orwell', 3, 3),
            ('To Kill a Mockingbird', 'Harper Lee', 4, 4);";

            using var insertCommand = new SqliteCommand(insertQuery, connection);
            insertCommand.ExecuteNonQuery();
        }

        public void SeedUsers()
        {
            using var connection = new SqliteConnection(SQL_connectionString);
            connection.Open();

            using var countCommand = new SqliteCommand("SELECT COUNT(*) FROM Users", connection);
            long count = (long)countCommand.ExecuteScalar()!;
            if (count > 0)
            {
                return;
            }

            var users = new[]
            {
        new { Username = "student1", Password = "student123", Role = "Student" },
        new { Username = "librarian1", Password = "librarian123", Role = "Librarian" }
    };

            foreach (var user in users)
            {
                string passwordHash = _passwordHasher.Hash(user.Password);
                string salt = ""; // Saltet ligger allerede i hashen

                var insertQuery = @"
            INSERT INTO Users (Username, PasswordHash, Salt, Role)
            VALUES (@Username, @PasswordHash, @Salt, @Role);";
                using var insertCommand = new SqliteCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@Username", user.Username);
                insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                insertCommand.Parameters.AddWithValue("@Salt", salt);
                insertCommand.Parameters.AddWithValue("@Role", user.Role);
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}