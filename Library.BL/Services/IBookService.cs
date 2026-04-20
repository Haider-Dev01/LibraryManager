using Library.BL.DTOs;

namespace Library.BL.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<BookDto?> GetBookWithLoansAsync(int id);
        Task<(bool Success, string Message, BookDto? Book)> CreateBookAsync(CreateBookDto dto);
        Task<(bool Success, string Message, BookDto? Book)> UpdateBookAsync(UpdateBookDto dto);
        Task<(bool Success, string Message)> DeleteBookAsync(int id);
    }
}