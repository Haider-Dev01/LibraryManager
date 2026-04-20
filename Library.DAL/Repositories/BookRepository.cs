using Library.DAL.Context;
using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Loans)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> GetByIdWithLoansAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Loans.OrderByDescending(l => l.LoanDate))
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> IsISBNUniqueAsync(string isbn, int? excludeId = null)
        {
            return !await _context.Books
                .AnyAsync(b => b.ISBN == isbn && (!excludeId.HasValue || b.Id != excludeId.Value));
        }
    }
}