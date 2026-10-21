using SchoolLibrary.Models;
using SchoolLibrary.Repositories;

namespace SchoolLibrary.Services
{
    public class LoanService : ILoanService
    {
        private const int LoanDays = 14;

        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;

        public LoanService(IBookRepository bookRepository, ILoanRepository loanRepository)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
        }

        public LoanResult BorrowBook(string username, int bookId)
        {
            var book = _bookRepository.GetById(bookId);
            if (book is null)
            {
                return new LoanResult { Success = false, Message = "Bogen findes ikke." };
            }

            if (book.Available <= 0)
            {
                return new LoanResult { Success = false, Message = $"\"{book.Title}\" er udlånt." };
            }

            var now = DateTime.Now;
            var loan = new Loan
            {
                BookId = bookId,
                Username = username,
                BorrowedAt = now,
                DueDate = now.AddDays(LoanDays)
            };

            _loanRepository.Create(loan);
            _bookRepository.UpdateAvailable(bookId, book.Available - 1);

            return new LoanResult
            {
                Success = true,
                Message = $"Du har lånt \"{book.Title}\". Afleveres senest {loan.DueDate:dd-MM-yyyy}.",
                DueDate = loan.DueDate
            };
        }
    }
}