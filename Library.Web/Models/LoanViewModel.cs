using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class LoanViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Livre")]
        public string BookTitle { get; set; } = string.Empty;

        [Display(Name = "Auteur")]
        public string BookAuthor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom de l'emprunteur est obligatoire")]
        [StringLength(150)]
        [Display(Name = "Emprunteur")]
        public string BorrowerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format email invalide")]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string BorrowerEmail { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date d'emprunt")]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [Display(Name = "Date de retour prévue")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "Retourné")]
        public bool IsReturned { get; set; }

        [Display(Name = "Jours d'emprunt")]
        public int DaysOnLoan { get; set; }
    }
}