using Library.BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILoanService _loanService;

        public HomeController(IBookService bookService, ILoanService loanService)
        {
            _bookService = bookService;
            _loanService = loanService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            var loans = await _loanService.GetAllLoansAsync();

            ViewBag.TotalBooks = books.Count();
            ViewBag.AvailableBooks = books.Count(b => b.IsAvailable);
            ViewBag.ActiveLoans = loans.Count(l => !l.IsReturned);
            ViewBag.TotalLoans = loans.Count();
            ViewBag.RecentLoans = loans.Take(5).ToList();

            return View();
        }
    }
}