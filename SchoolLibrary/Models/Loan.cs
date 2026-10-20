namespace SchoolLibrary.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string Username { get; set; } = "";

        public DateTime BorrowedAt { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsReturned => ReturnedAt.HasValue;

        public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

        public bool IsLate => IsReturned && ReturnedAt.Value > DueDate;
    }
}