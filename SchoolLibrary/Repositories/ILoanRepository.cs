using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public interface ILoanRepository
    {
        void Create(Loan loan);
        Loan? GetById(int id);
        void Return(int loanId, DateTime returnedAt);
        List<Loan> GetByUser(string username);
    }
}