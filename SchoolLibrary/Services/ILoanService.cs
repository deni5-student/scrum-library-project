namespace SchoolLibrary.Services
{
    public interface ILoanService
    {
        LoanResult BorrowBook(string username, int bookId);
    }
}