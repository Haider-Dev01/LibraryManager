using Library.DAL.Entities;

namespace Library.DAL.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId);
        Task<Loan?> GetByIdAsync(int id);
        Task<Loan?> GetByIdWithBookAsync(int id);
        Task<Loan> CreateAsync(Loan loan);
        Task<Loan> UpdateAsync(Loan loan);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasActiveLoanAsync(int bookId);
    }
}