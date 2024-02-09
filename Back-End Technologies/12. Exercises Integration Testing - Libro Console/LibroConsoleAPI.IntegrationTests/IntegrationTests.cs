using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests
{
    public class IntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public IntegrationTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
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
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal("Test Book", bookInDb.Title);
            Assert.Equal("John Doe", bookInDb.Author);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidCredentials_ShouldThrowException()
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
            await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook));
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
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

            await _dbContext.Books.AddAsync(bookToAdd);
            await _dbContext.SaveChangesAsync();

            // Act
            await _bookManager.DeleteAsync("1234567890123");

            // Assert
            var deletedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == "1234567890123");
            Assert.Null(deletedBook);
        }
        [Fact]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange
            string nullOrWhiteSpaceISBN = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookManager.DeleteAsync(nullOrWhiteSpaceISBN));
        }

        [Fact]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange
            var booksToAdd = new List<Book>
    {
        new Book
        {
            Title = "Book 1",
            Author = "Author 1",
            ISBN = "ISBN 1",
            YearPublished = 2021,
            Genre = "Genre 1",
            Pages = 200,
            Price = 20.99
        },
        new Book
        {
            Title = "Book 2",
            Author = "Author 2",
            ISBN = "ISBN 2",
            YearPublished = 2022,
            Genre = "Genre 2",
            Pages = 250,
            Price = 25.99
        }
    };

            foreach (var book in booksToAdd)
            {
                await _dbContext.Books.AddAsync(book);
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var allBooks = await _bookManager.GetAllAsync();

            // Assert
            Assert.NotNull(allBooks);
            Assert.Equal(booksToAdd.Count, allBooks.Count());
            foreach (var book in booksToAdd)
            {
                Assert.Contains(book, allBooks);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(async () => await _bookManager.GetAllAsync());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        [Fact]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange
            var booksToAdd = new List<Book>
    {
        new Book
        {
            Title = "Book with Valid Title Fragment 1",
            Author = "Author 1",
            ISBN = "ISBN 1",
            YearPublished = 2021,
            Genre = "Genre 1",
            Pages = 200,
            Price = 20.99
        },
        new Book
        {
            Title = "Book with Valid Title Fragment 2",
            Author = "Author 2",
            ISBN = "ISBN 2",
            YearPublished = 2022,
            Genre = "Genre 2",
            Pages = 250,
            Price = 25.99
        }
    };

            foreach (var book in booksToAdd)
            {
                await _dbContext.Books.AddAsync(book);
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var matchingBooks = await _bookManager.SearchByTitleAsync("Valid Title Fragment");

            // Assert
            Assert.NotNull(matchingBooks);
            Assert.Equal(2, matchingBooks.Count());
        }

        [Fact]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Act
            var exception = await Record.ExceptionAsync(async () => await _bookManager.SearchByTitleAsync("Invalid Title Fragment"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        [Fact]
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

            await _dbContext.Books.AddAsync(bookToAdd);
            await _dbContext.SaveChangesAsync();

            // Act
            var retrievedBook = await _bookManager.GetSpecificAsync("1234567890123");

            // Assert
            Assert.NotNull(retrievedBook);
            Assert.Equal("Sample Book", retrievedBook.Title);
            Assert.Equal("John Smith", retrievedBook.Author);
        }

        [Fact]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Act
            var exception = await Record.ExceptionAsync(async () => await _bookManager.GetSpecificAsync("Invalid ISBN"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }

        [Fact]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
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

            await _dbContext.Books.AddAsync(bookToAdd);
            await _dbContext.SaveChangesAsync();

            bookToAdd.Title = "Updated Sample Book";
            bookToAdd.Author = "Updated Author";
            bookToAdd.YearPublished = 2023;
            bookToAdd.Genre = "Fantasy";
            bookToAdd.Pages = 350;
            bookToAdd.Price = 19.99;

            // Act
            await _bookManager.UpdateAsync(bookToAdd);

            // Assert
            var updatedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == bookToAdd.ISBN);
            Assert.NotNull(updatedBook);
            Assert.Equal("Updated Sample Book", updatedBook.Title);
            Assert.Equal("Updated Author", updatedBook.Author);
            Assert.Equal(2023, updatedBook.YearPublished);
            Assert.Equal("Fantasy", updatedBook.Genre);
            Assert.Equal(350, updatedBook.Pages);
            Assert.Equal(19.99, updatedBook.Price);
        }

        [Fact]
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
            await Assert.ThrowsAsync<ValidationException>(() => _bookManager.UpdateAsync(invalidBook));
        }

    }
}
