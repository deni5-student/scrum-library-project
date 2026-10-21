namespace SchoolLibrary.Services
{
    public interface ILoanService
    {
        LoanResult BorrowBook(string username, int bookId);
        LoanResult ReturnBook(string username, int loanId);
    }
}