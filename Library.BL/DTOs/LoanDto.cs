namespace Library.BL.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public string BorrowerEmail { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public int DaysOnLoan { get; set; }
    }

    public class CreateLoanDto
    {
        public int BookId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public string BorrowerEmail { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; }
    }
}