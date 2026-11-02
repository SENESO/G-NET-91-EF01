using EF01.Data;
using EF01.Models;
using Microsoft.EntityFrameworkCore;

namespace EF01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BookstoreContext())
            {
                bool created = context.Database.EnsureCreated();
                if (created)
                    Console.WriteLine("Database 'ReadMoreBooks' was created successfully.");
                else
                    Console.WriteLine("Database 'ReadMoreBooks' already exists.");

                if (!context.Authors.Any())
                {
                    var author1 = new Author
                    {
                        FirstName = "Naguib",
                        LastName = "Mahfouz",
                        Email = "naguib.mahfouz@example.com",
                        Bio = "Egyptian novelist and Nobel Prize winner in Literature.",
                        DateOfBirth = new DateTime(1911, 12, 11)
                    };
                    var author2 = new Author
                    {
                        FirstName = "Taha",
                        LastName = "Hussein",
                        Email = "taha.hussein@example.com",
                        Bio = "Egyptian writer and intellectual, known as the Dean of Arabic Literature.",
                        DateOfBirth = new DateTime(1889, 11, 14)
                    };

                    var fiction = new Category
                    {
                        Name = "Fiction",
                        Description = "Novels and short stories.",
                        IsActive = true
                    };
                    var history = new Category
                    {
                        Name = "History",
                        Description = "Books about historical events and figures.",
                        IsActive = true
                    };

                    var book1 = new Book
                    {
                        Title = "Palace Walk",
                        ISBN = "9780385264779",
                        Price = 180,
                        Pages = 512,
                        PublishedYear = 1956,
                        IsInStock = true,
                        Author = author1,
                        Category = fiction
                    };
                    var book2 = new Book
                    {
                        Title = "Midaq Alley",
                        ISBN = "9780385264762",
                        Price = 150,
                        Pages = 286,
                        PublishedYear = 1947,
                        IsInStock = true,
                        Author = author1,
                        Category = fiction
                    };
                    var book3 = new Book
                    {
                        Title = "The Days",
                        ISBN = "9789774248599",
                        Price = 120,
                        Pages = 320,
                        PublishedYear = 1929,
                        IsInStock = false,
                        Author = author2,
                        Category = history
                    };

                    context.Authors.AddRange(author1, author2);
                    context.Categories.AddRange(fiction, history);
                    context.Books.AddRange(book1, book2, book3);
                    context.SaveChanges();
                    Console.WriteLine("Sample data was added.");
                }

                Console.WriteLine();
                Console.WriteLine("Books in the store:");
                var books = context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category);
                foreach (var book in books)
                {
                    Console.WriteLine($"{book.Title} - {book.Author.FirstName} {book.Author.LastName} - {book.Category.Name} - {book.Price} - In Stock: {book.IsInStock}");
                }

                Console.WriteLine();
                Console.WriteLine("Tables created in the database:");
                var connection = context.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Console.WriteLine("- " + reader.GetString(0));
                }
                connection.Close();
            }
        }
    }
}
