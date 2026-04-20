using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.DAL.Context
{
    public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();

            // Configure ici la chaîne de connexion pour l'outillage
            // Il est préférable de ne pas la "hardcoder". On la récupère depuis l'app Web.
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(connectionString);

            return new LibraryDbContext(optionsBuilder.Options);
        }
    }
}