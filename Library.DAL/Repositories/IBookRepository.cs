using Library.DAL.Entities;

namespace Library.DAL.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByIdWithLoansAsync(int id);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IsISBNUniqueAsync(string isbn, int? excludeId = null);
    }
}