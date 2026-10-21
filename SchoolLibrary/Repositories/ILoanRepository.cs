using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public interface ILoanRepository
    {
        void Create(Loan loan);
    }
}