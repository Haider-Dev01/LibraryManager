using Library.DAL.Context;
using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;

        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.BookId == bookId)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task<Loan?> GetByIdWithBookAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Loan> CreateAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan> UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return false;

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasActiveLoanAsync(int bookId)
        {
            return await _context.Loans
                .AnyAsync(l => l.BookId == bookId && !l.IsReturned);
        }
    }
}