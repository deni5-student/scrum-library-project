using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string SQL_connectionString;

        public BookRepository(IConfiguration configuration)
        {
            SQL_connectionString = configuration.GetConnectionString("Default");
        }

        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();

            using var connection = new SqliteConnection(SQL_connectionString);
            connection.Open();

            var query = "SELECT Id, Title, Author, Total, Available FROM Books";

            using var command = new SqliteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var book = new Book
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    Total = reader.GetInt32(3),
                    Available = reader.GetInt32(4)
                };

                books.Add(book);
            }

            return books;
        }
    }
}
