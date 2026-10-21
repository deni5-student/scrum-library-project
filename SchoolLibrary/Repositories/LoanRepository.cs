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
    }
}