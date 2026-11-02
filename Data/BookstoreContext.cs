using EF01.Models;
using Microsoft.EntityFrameworkCore;

namespace EF01.Data
{
    public class BookstoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=ReadMoreBooks;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
