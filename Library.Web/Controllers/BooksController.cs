using Library.BL.DTOs;
using Library.BL.Services;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILoanService _loanService;

        public BooksController(IBookService bookService, ILoanService loanService)
        {
            _bookService = bookService;
            _loanService = loanService;
        }

        // GET: /Books
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            var bookVMs = books.Select(MapToViewModel).ToList();

            var viewModel = new BookListViewModel
            {
                Books = bookVMs,
                TotalBooks = bookVMs.Count,
                AvailableBooks = bookVMs.Count(b => b.IsAvailable),
                BorrowedBooks = bookVMs.Count(b => !b.IsAvailable)
            };

            return View(viewModel);
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookWithLoansAsync(id);
            if (book == null) return NotFound();

            var loans = await _loanService.GetLoansByBookAsync(id);
            ViewBag.Loans = loans.Select(l => new LoanViewModel
            {
                Id = l.Id,
                BookId = l.BookId,
                BookTitle = l.BookTitle,
                BookAuthor = l.BookAuthor,
                BorrowerName = l.BorrowerName,
                BorrowerEmail = l.BorrowerEmail,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                IsReturned = l.IsReturned,
                DaysOnLoan = l.DaysOnLoan
            }).ToList();

            return View(MapToViewModel(book));
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            return View(new BookViewModel { PublicationYear = DateTime.Now.Year });
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new CreateBookDto
            {
                Title = model.Title,
                Author = model.Author,
                ISBN = model.ISBN,
                PublicationYear = model.PublicationYear,
                Genre = model.Genre
            };

            var (success, message, _) = await _bookService.CreateBookAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return View(MapToViewModel(book));
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var dto = new UpdateBookDto
            {
                Id = model.Id,
                Title = model.Title,
                Author = model.Author,
                ISBN = model.ISBN,
                PublicationYear = model.PublicationYear,
                Genre = model.Genre,
                IsAvailable = model.IsAvailable
            };

            var (success, message, _) = await _bookService.UpdateBookAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return View(MapToViewModel(book));
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, message) = await _bookService.DeleteBookAsync(id);

            if (!success)
            {
                TempData["Error"] = message;
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        private static BookViewModel MapToViewModel(BookDto dto) => new BookViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            PublicationYear = dto.PublicationYear,
            Genre = dto.Genre,
            IsAvailable = dto.IsAvailable,
            TotalLoans = dto.TotalLoans,
            ActiveLoans = dto.ActiveLoans
        };
    }
}