using Microsoft.Data.Sqlite;
using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly string _connectionString;

        public LoanRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        }

        public void Create(Loan loan)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(
                @"INSERT INTO Loans (BookId, Username, BorrowedAt, DueDate)
                  VALUES (@bookId, @username, @borrowedAt, @dueDate)",
                connection);
            command.Parameters.AddWithValue("@bookId", loan.BookId);
            command.Parameters.AddWithValue("@username", loan.Username);
            command.Parameters.AddWithValue("@borrowedAt", loan.BorrowedAt.ToString("o"));
            command.Parameters.AddWithValue("@dueDate", loan.DueDate.ToString("o"));
            command.ExecuteNonQuery();
        }
        public Loan? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(
                @"SELECT Id, BookId, Username, BorrowedAt, DueDate, ReturnedAt
          FROM Loans WHERE Id = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new Loan
            {
                Id = reader.GetInt32(0),
                BookId = reader.GetInt32(1),
                Username = reader.GetString(2),
                BorrowedAt = DateTime.Parse(reader.GetString(3)),
                DueDate = DateTime.Parse(reader.GetString(4)),
                ReturnedAt = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5))
            };
        }

        public void Return(int loanId, DateTime returnedAt)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(
                "UPDATE Loans SET ReturnedAt = @returnedAt WHERE Id = @id",
                connection);
            command.Parameters.AddWithValue("@returnedAt", returnedAt.ToString("o"));
            command.Parameters.AddWithValue("@id", loanId);
            command.ExecuteNonQuery();
        }

        public List<Loan> GetByUser(string username)
        {
            var loans = new List<Loan>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var command = new SqliteCommand(
                @"SELECT Id, BookId, Username, BorrowedAt, DueDate, ReturnedAt
          FROM Loans WHERE Username = @username
          ORDER BY BorrowedAt DESC",
                connection);
            command.Parameters.AddWithValue("@username", username);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                loans.Add(new Loan
                {
                    Id = reader.GetInt32(0),
                    BookId = reader.GetInt32(1),
                    Username = reader.GetString(2),
                    BorrowedAt = DateTime.Parse(reader.GetString(3)),
                    DueDate = DateTime.Parse(reader.GetString(4)),
                    ReturnedAt = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5))
                });
            }

            return loans;
        }
    }
}