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
        public LoanResult ReturnBook(string username, int loanId)
        {
            var loan = _loanRepository.GetById(loanId);
            if (loan is null || loan.Username != username)
            {
                return new LoanResult { Success = false, Message = "Lånet blev ikke fundet." };
            }

            if (loan.IsReturned)
            {
                return new LoanResult { Success = false, Message = "Bogen er allerede afleveret." };
            }

            var now = DateTime.Now;
            _loanRepository.Return(loanId, now);

            var book = _bookRepository.GetById(loan.BookId);
            if (book is not null)
            {
                _bookRepository.UpdateAvailable(book.Id, book.Available + 1);
            }

            var title = book?.Title ?? "Bogen";

            if (now > loan.DueDate)
            {
                return new LoanResult
                {
                    Success = false,
                    Message = $"\"{title}\" er afleveret for sent. Afleveringsdatoen var {loan.DueDate:dd-MM-yyyy}."
                };
            }

            return new LoanResult
            {
                Success = true,
                Message = $"\"{title}\" er afleveret til tiden. ✔"
            };
        }
    }
}