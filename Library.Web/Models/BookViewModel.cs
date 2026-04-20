using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'auteur est obligatoire")]
        [StringLength(150)]
        [Display(Name = "Auteur")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'ISBN est obligatoire")]
        [StringLength(20)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'année de publication est obligatoire")]
        [Range(1000, 2100, ErrorMessage = "Année invalide")]
        [Display(Name = "Année de publication")]
        public int PublicationYear { get; set; } = DateTime.Now.Year;

        [Display(Name = "Genre")]
        [StringLength(100)]
        public string Genre { get; set; } = string.Empty;

        [Display(Name = "Disponible")]
        public bool IsAvailable { get; set; } = true;

        public int TotalLoans { get; set; }
        public int ActiveLoans { get; set; }
    }

    public class BookListViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; } = new List<BookViewModel>();
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
    }
}