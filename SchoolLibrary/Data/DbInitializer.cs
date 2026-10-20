using Microsoft.Data.Sqlite;

namespace SchoolLibrary.Data
    {
        public class DbInitializer
        {
            private readonly string SQL_connectionString;

            public DbInitializer(IConfiguration configuration)
            {
                SQL_connectionString = configuration.GetConnectionString("Default")
                    ?? throw new InvalidOperationException("Connection string 'Default' not found.");
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
    }
}