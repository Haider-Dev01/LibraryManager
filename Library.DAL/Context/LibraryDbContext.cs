using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Author).IsRequired().HasMaxLength(150);
                entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
                entity.Property(b => b.Genre).HasMaxLength(100);
                entity.HasIndex(b => b.ISBN).IsUnique();
            });

            // Configuration Loan
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.BorrowerName).IsRequired().HasMaxLength(150);
                entity.Property(l => l.BorrowerEmail).IsRequired().HasMaxLength(200);
                entity.Property(l => l.LoanDate).IsRequired();

                // Relation : un Book a plusieurs Loans
                entity.HasOne(l => l.Book)
                      .WithMany(b => b.Loans)
                      .HasForeignKey(l => l.BookId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed Data
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", PublicationYear = 2008, Genre = "Informatique", IsAvailable = true },
                new Book { Id = 2, Title = "Le Petit Prince", Author = "Antoine de Saint-Exupéry", ISBN = "978-2070612758", PublicationYear = 1943, Genre = "Littérature", IsAvailable = true },
                new Book { Id = 3, Title = "1984", Author = "George Orwell", ISBN = "978-2070368228", PublicationYear = 1949, Genre = "Dystopie", IsAvailable = true }
            );
        }
    }
}