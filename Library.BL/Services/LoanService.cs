using Library.BL.DTOs;
using Library.DAL.Entities;
using Library.DAL.Repositories;

namespace Library.BL.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;

        public LoanService(ILoanRepository loanRepository, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<LoanDto>> GetAllLoansAsync()
        {
            var loans = await _loanRepository.GetAllAsync();
            return loans.Select(MapToDto);
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByBookAsync(int bookId)
        {
            var loans = await _loanRepository.GetByBookIdAsync(bookId);
            return loans.Select(MapToDto);
        }

        public async Task<LoanDto?> GetLoanByIdAsync(int id)
        {
            var loan = await _loanRepository.GetByIdWithBookAsync(id);
            return loan == null ? null : MapToDto(loan);
        }

        public async Task<(bool Success, string Message, LoanDto? Loan)> CreateLoanAsync(CreateLoanDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(dto.BookId);
            if (book == null)
                return (false, "Livre introuvable.", null);

            if (!book.IsAvailable)
                return (false, "Ce livre n'est pas disponible pour l'emprunt.", null);

            if (await _loanRepository.HasActiveLoanAsync(dto.BookId))
                return (false, "Ce livre est déjà emprunté.", null);

            var loan = new Loan
            {
                BookId = dto.BookId,
                BorrowerName = dto.BorrowerName,
                BorrowerEmail = dto.BorrowerEmail,
                LoanDate = dto.LoanDate,
                ReturnDate = dto.ReturnDate,
                IsReturned = false
            };

            // Marquer le livre comme non disponible
            book.IsAvailable = false;
            await _bookRepository.UpdateAsync(book);

            var created = await _loanRepository.CreateAsync(loan);
            var withBook = await _loanRepository.GetByIdWithBookAsync(created.Id);
            return (true, "Emprunt créé avec succès.", MapToDto(withBook!));
        }

        public async Task<(bool Success, string Message)> ReturnBookAsync(int loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
                return (false, "Emprunt introuvable.");

            if (loan.IsReturned)
                return (false, "Ce livre a déjà été retourné.");

            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;
            await _loanRepository.UpdateAsync(loan);

            // Remettre le livre disponible
            var book = await _bookRepository.GetByIdAsync(loan.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
                await _bookRepository.UpdateAsync(book);
            }

            return (true, "Livre retourné avec succès.");
        }

        public async Task<(bool Success, string Message)> DeleteLoanAsync(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
                return (false, "Emprunt introuvable.");

            if (!loan.IsReturned)
                return (false, "Impossible de supprimer un emprunt actif. Retournez d'abord le livre.");

            await _loanRepository.DeleteAsync(id);
            return (true, "Emprunt supprimé avec succès.");
        }

        private static LoanDto MapToDto(Loan loan)
        {
            var days = loan.IsReturned && loan.ReturnDate.HasValue
                ? (int)(loan.ReturnDate.Value - loan.LoanDate).TotalDays
                : (int)(DateTime.Now - loan.LoanDate).TotalDays;

            return new LoanDto
            {
                Id = loan.Id,
                BookId = loan.BookId,
                BookTitle = loan.Book?.Title ?? "Inconnu",
                BookAuthor = loan.Book?.Author ?? "Inconnu",
                BorrowerName = loan.BorrowerName,
                BorrowerEmail = loan.BorrowerEmail,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                IsReturned = loan.IsReturned,
                DaysOnLoan = days
            };
        }
    }
}