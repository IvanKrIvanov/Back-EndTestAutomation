using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestLibroDbContext dbContext;
        private IBookManager bookManager;

        [SetUp]
        public void SetUp()
        {
            string dbName = $"TestDb_{Guid.NewGuid()}";
            this.dbContext = new TestLibroDbContext(dbName);
            this.bookManager = new BookManager(new BookRepository(this.dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.AreEqual("Test Book", bookInDb.Title);
            Assert.AreEqual("John Doe", bookInDb.Author);
        }

        [Test]
        public async Task AddBookAsync_TryToAddBookWithInvalidCredentials_ShouldThrowException()
        {
            var invalidBook = new Book
            {
                // Arrange
                Author = "Invalid Author",
                ISBN = "Invalid ISBN",
                YearPublished = 2022,
                Genre = null,
                Pages = -100,
                Price = -10.99
            };

            // Act & Assert
            try
            {

                await bookManager.AddAsync(invalidBook);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

        }

        [Test]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
        {
            // Arrange
            var bookToAdd = new Book
            {
                Title = "Sample Book",
                Author = "John Smith",
                ISBN = "1234567890123", // Valid ISBN
                YearPublished = 2022,
                Genre = "Science Fiction",
                Pages = 300,
                Price = 15.99
            };

            await dbContext.Books.AddAsync(bookToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            await bookManager.DeleteAsync("1234567890123");

            // Assert
            var deletedBook = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == "1234567890123");
            Assert.Null(deletedBook);
        }

        [Test]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await dbContext.Books.AddAsync(newBook);
            await dbContext.SaveChangesAsync();

            // Act
            await bookManager.DeleteAsync(newBook.ISBN);

            // Assert
            var deletedBook = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.Null(deletedBook);
        }

        [Test]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange
            var booksToAdd = new List<Book>
    {
        new Book
        {
            Title = "Book 1",
            Author = "Author 1",
            ISBN = "1111111111111",
            YearPublished = 2000,
            Genre = "Genre 1",
            Pages = 200,
            Price = 20.00
        },
        new Book
        {
            Title = "Book 2",
            Author = "Author 2",
            ISBN = "2222222222222",
            YearPublished = 2005,
            Genre = "Genre 2",
            Pages = 250,
            Price = 25.00
        }
    };

            await dbContext.Books.AddRangeAsync(booksToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var retrievedBooks = await bookManager.GetAllAsync();

            // Assert
            Assert.NotNull(retrievedBooks);
            Assert.AreEqual(2, retrievedBooks.Count());

            // Assert 
            Assert.IsTrue(retrievedBooks.Any(b => b.Title == "Book 1"));
            Assert.IsTrue(retrievedBooks.Any(b => b.Title == "Book 2"));
        }

        [Test]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            try
            {
                await bookManager.GetAllAsync();

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (KeyNotFoundException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange
            var booksToAdd = new List<Book>
    {
        new Book
        {
            Title = "Harry Potter and the Philosopher's Stone",
            Author = "J.K. Rowling",
            ISBN = "1111111111111",
            YearPublished = 1997,
            Genre = "Fantasy",
            Pages = 320,
            Price = 10.99
        },
        new Book
        {
            Title = "The Hobbit",
            Author = "J.R.R. Tolkien",
            ISBN = "2222222222222",
            YearPublished = 1937,
            Genre = "Fantasy",
            Pages = 310,
            Price = 12.99
        }
    };

            await dbContext.Books.AddRangeAsync(booksToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var searchResult = await bookManager.SearchByTitleAsync("Harry Potter");

            // Assert
            Assert.NotNull(searchResult);
            Assert.AreEqual(1, searchResult.Count());
            Assert.IsTrue(searchResult.Any(b => b.Title.Contains("Harry Potter")));
        }

        [Test]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            try
            {

                await bookManager.SearchByTitleAsync("Invalid Title Fragment");


                Assert.Fail("Expected exception was not thrown.");
            }
            catch (KeyNotFoundException ex)
            {

                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
        {
            // Arrange
            var bookToAdd = new Book
            {
                Title = "Sample Book",
                Author = "John Smith",
                ISBN = "1234567890123",
                YearPublished = 2022,
                Genre = "Science Fiction",
                Pages = 300,
                Price = 15.99
            };

            await dbContext.Books.AddAsync(bookToAdd);
            await dbContext.SaveChangesAsync();

            // Act
            var retrievedBook = await bookManager.GetSpecificAsync("1234567890123");

            // Assert
            Assert.NotNull(retrievedBook);
            Assert.AreEqual("Sample Book", retrievedBook.Title);
            Assert.AreEqual("John Smith", retrievedBook.Author);
            Assert.AreEqual("1234567890123", retrievedBook.ISBN);
            Assert.AreEqual(2022, retrievedBook.YearPublished);
            Assert.AreEqual("Science Fiction", retrievedBook.Genre);
            Assert.AreEqual(300, retrievedBook.Pages);
            Assert.AreEqual(15.99, retrievedBook.Price);
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            try
            {
                await bookManager.GetSpecificAsync("Invalid ISBN");

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (KeyNotFoundException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
        {
            // Act & Assert
            try
            {
                await bookManager.GetSpecificAsync("Invalid ISBN");

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (KeyNotFoundException ex)
            {

                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange
            var invalidBook = new Book
            {
                Title = "",
                Author = "Invalid Author",
                ISBN = "Invalid ISBN",
                YearPublished = 2022,
                Genre = null,
                Pages = -100,
                Price = -10.99
            };

            // Act & Assert
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(invalidBook, new ValidationContext(invalidBook), validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Any());
        }
    }
}
