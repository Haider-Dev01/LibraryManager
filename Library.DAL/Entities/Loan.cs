namespace Library.DAL.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public string BorrowerEmail { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

        // Navigation property
        public Book Book { get; set; } = null!;
    }
}