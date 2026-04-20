using Library.BL.DTOs;

namespace Library.BL.Services
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDto>> GetAllLoansAsync();
        Task<IEnumerable<LoanDto>> GetLoansByBookAsync(int bookId);
        Task<LoanDto?> GetLoanByIdAsync(int id);
        Task<(bool Success, string Message, LoanDto? Loan)> CreateLoanAsync(CreateLoanDto dto);
        Task<(bool Success, string Message)> ReturnBookAsync(int loanId);
        Task<(bool Success, string Message)> DeleteLoanAsync(int id);
    }
}