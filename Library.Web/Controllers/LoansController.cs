using Library.BL.DTOs;
using Library.BL.Services;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Controllers
{
    public class LoansController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IBookService _bookService;

        public LoansController(ILoanService loanService, IBookService bookService)
        {
            _loanService = loanService;
            _bookService = bookService;
        }

        // GET: /Loans
        public async Task<IActionResult> Index()
        {
            var loans = await _loanService.GetAllLoansAsync();
            var vms = loans.Select(MapToViewModel).ToList();
            return View(vms);
        }

        // GET: /Loans/Create?bookId=3
        public async Task<IActionResult> Create(int? bookId)
        {
            var books = await _bookService.GetAllBooksAsync();
            var availableBooks = books.Where(b => b.IsAvailable).ToList();

            ViewBag.Books = new SelectList(
                availableBooks,
                "Id",
                "Title",
                bookId
            );

            return View(new LoanViewModel
            {
                BookId = bookId ?? 0,
                LoanDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(14)
            });
        }

        // POST: /Loans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateBooksSelectList(model.BookId);
                return View(model);
            }

            var dto = new CreateLoanDto
            {
                BookId = model.BookId,
                BorrowerName = model.BorrowerName,
                BorrowerEmail = model.BorrowerEmail,
                LoanDate = model.LoanDate,
                ReturnDate = model.ReturnDate
            };

            var (success, message, loan) = await _loanService.CreateLoanAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                await PopulateBooksSelectList(model.BookId);
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // POST: /Loans/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var (success, message) = await _loanService.ReturnBookAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Loans/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null) return NotFound();
            return View(MapToViewModel(loan));
        }

        // POST: /Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, message) = await _loanService.DeleteLoanAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateBooksSelectList(int selectedId = 0)
        {
            var books = await _bookService.GetAllBooksAsync();
            ViewBag.Books = new SelectList(
                books.Where(b => b.IsAvailable),
                "Id", "Title", selectedId);
        }

        private static LoanViewModel MapToViewModel(LoanDto dto) => new LoanViewModel
        {
            Id = dto.Id,
            BookId = dto.BookId,
            BookTitle = dto.BookTitle,
            BookAuthor = dto.BookAuthor,
            BorrowerName = dto.BorrowerName,
            BorrowerEmail = dto.BorrowerEmail,
            LoanDate = dto.LoanDate,
            ReturnDate = dto.ReturnDate,
            IsReturned = dto.IsReturned,
            DaysOnLoan = dto.DaysOnLoan
        };
    }
}