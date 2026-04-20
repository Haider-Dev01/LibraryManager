using Library.BL.DTOs;
using Library.DAL.Entities;
using Library.DAL.Repositories;

namespace Library.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;

        public BookService(IBookRepository bookRepository, ILoanRepository loanRepository)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(b => MapToDto(b));
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book == null ? null : MapToDto(book);
        }

        public async Task<BookDto?> GetBookWithLoansAsync(int id)
        {
            var book = await _bookRepository.GetByIdWithLoansAsync(id);
            return book == null ? null : MapToDto(book);
        }

        public async Task<(bool Success, string Message, BookDto? Book)> CreateBookAsync(CreateBookDto dto)
        {
            // Validation ISBN unique
            if (!await _bookRepository.IsISBNUniqueAsync(dto.ISBN))
                return (false, $"Un livre avec l'ISBN '{dto.ISBN}' existe déjà.", null);

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                PublicationYear = dto.PublicationYear,
                Genre = dto.Genre,
                IsAvailable = true
            };

            var created = await _bookRepository.CreateAsync(book);
            return (true, "Livre créé avec succès.", MapToDto(created));
        }

        public async Task<(bool Success, string Message, BookDto? Book)> UpdateBookAsync(UpdateBookDto dto)
        {
            var book = await _bookRepository.GetByIdAsync(dto.Id);
            if (book == null)
                return (false, "Livre introuvable.", null);

            if (!await _bookRepository.IsISBNUniqueAsync(dto.ISBN, dto.Id))
                return (false, $"Un autre livre avec l'ISBN '{dto.ISBN}' existe déjà.", null);

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.PublicationYear = dto.PublicationYear;
            book.Genre = dto.Genre;
            book.IsAvailable = dto.IsAvailable;

            var updated = await _bookRepository.UpdateAsync(book);
            return (true, "Livre modifié avec succès.", MapToDto(updated));
        }

        public async Task<(bool Success, string Message)> DeleteBookAsync(int id)
        {
            if (!await _bookRepository.ExistsAsync(id))
                return (false, "Livre introuvable.");

            if (await _loanRepository.HasActiveLoanAsync(id))
                return (false, "Impossible de supprimer : ce livre a un emprunt actif.");

            await _bookRepository.DeleteAsync(id);
            return (true, "Livre supprimé avec succès.");
        }

        private static BookDto MapToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Genre = book.Genre,
                IsAvailable = book.IsAvailable,
                TotalLoans = book.Loans?.Count ?? 0,
                ActiveLoans = book.Loans?.Count(l => !l.IsReturned) ?? 0
            };
        }
    }
}