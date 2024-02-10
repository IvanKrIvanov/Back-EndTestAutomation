using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MoviesLibraryAPI.Controllers;
using MoviesLibraryAPI.Controllers.Contracts;
using MoviesLibraryAPI.Data.Models;
using MoviesLibraryAPI.Services;
using MoviesLibraryAPI.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MoviesLibraryAPI.XUnitTests
{
    public class XUnitIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly MoviesLibraryXUnitTestDbContext _dbContext;
        private readonly IMoviesLibraryController _controller;
        private readonly IMoviesRepository _repository;

        public XUnitIntegrationTests(DatabaseFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _repository = new MoviesRepository(_dbContext.Movies);
            _controller = new MoviesLibraryController(_repository);
        }

        [Fact]
        public async Task AddMovieAsync_WhenValidMovieProvided_ShouldAddToDatabase()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };

            // Act
            await _controller.AddAsync(movie);

            // Assert
            var resultMovie = await _dbContext.Movies.Find(m => m.Title == "Test Movie").FirstOrDefaultAsync();
            Assert.NotNull(resultMovie);
            Assert.Equal("Test Movie", resultMovie.Title);
            Assert.Equal("Test Director", resultMovie.Director);
            Assert.Equal(2022, resultMovie.YearReleased);
            Assert.Equal("Action", resultMovie.Genre);
            Assert.Equal(120, resultMovie.Duration);
            Assert.Equal(7.5, resultMovie.Rating);
        }

        [Fact]
        public async Task AddMovieAsync_WhenInvalidMovieProvided_ShouldThrowValidationException()
        {
            // Arrange
            var invalidMovie = new Movie
            {
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };
            {
                // Provide an invalid movie object, e.g., without a title or other required fields
            };

            // Act and Assert
            var exception = await Xunit.Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(invalidMovie));
            Assert.Equal("Movie is not valid.", exception.Message);

        }

        [Fact]
        public async Task DeleteAsync_WhenValidTitleProvided_ShouldDeleteMovie()
        {
            // Arrange            
            var movie = new Movie
            {
                Title = "Test Second Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };

            await _controller.AddAsync(movie);
            // Act
            await _controller.DeleteAsync(movie.Title);

            // Assert
            // The movie should no longer exist in the database
            var resultMovie = await _dbContext.Movies.Find(m => m.Title == movie.Title).FirstOrDefaultAsync();
            Xunit.Assert.Null(resultMovie);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public async Task DeleteAsync_WhenTitleIsNull_ShouldThrowArgumentException(string invalidName)
        {
            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(() => _controller.DeleteAsync(invalidName));
        }

        [Fact]
        public async Task DeleteAsync_WhenTitleIsEmpty_ShouldThrowArgumentException()
        {
            // Act and Assert
        }

        [Fact]
        public async Task DeleteAsync_WhenTitleDoesNotExist_ShouldThrowInvalidOperationException()
        {
            // Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeleteAsync("Invalid Name"));
        }

        [Fact]
        public async Task GetAllAsync_WhenNoMoviesExist_ShouldReturnEmptyList()
        {
            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WhenMoviesExist_ShouldReturnAllMovies()
        {
            // Arrange
            var firstMovie = new Movie
            {
                Title = "GetAll First Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };

            var SecondMovie = new Movie
            {
                Title = "GetAll Second Movie",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { firstMovie, SecondMovie });

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            // Ensure that all movies are returned
            Xunit.Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByTitle_WhenTitleExists_ShouldReturnMatchingMovie()
        {
            // Arrange
            var firstMovie = new Movie
            {
                Title = "GetAll First Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };

            var SecondMovie = new Movie
            {
                Title = "GetAll Second Movie",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { firstMovie, SecondMovie });

            // Act
            var result = await _controller.GetByTitle(firstMovie.Title);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(firstMovie.Director, result.Director);
            Assert.Equal(firstMovie.Title, result.Title);
            Assert.Equal(firstMovie.Genre, result.Genre);
        }

        [Fact]
        public async Task GetByTitle_WhenTitleDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = await _controller.GetByTitle("Non Existing Title");

            // Assert
            Xunit.Assert.Null(result);
        }


        [Fact]
        public async Task SearchByTitleFragmentAsync_WhenTitleFragmentExists_ShouldReturnMatchingMovies()
        {
            // Arrange
            var firstMovie = new Movie
            {
                Title = "My Search Fragment First Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };

            var secondMovie = new Movie
            {
                Title = "My Search Fragment Second Movie",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { firstMovie, secondMovie });

            // Act
            var result = await _controller.SearchByTitleFragmentAsync("Your");

            // Assert // Should return one matching movie
            Assert.Equal(1, result.Count());
            var movieResult = result.First();
            Assert.Equal(secondMovie.Title, movieResult.Title);
            Assert.Equal(secondMovie.Genre, movieResult.Genre);

        }

        [Fact]
        public async Task SearchByTitleFragmentAsync_WhenNoMatchingTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(()=> _controller.SearchByTitleFragmentAsync("NoExistingFragments"));
        }

        [Fact]
        public async Task UpdateAsync_WhenValidMovieProvided_ShouldUpdateMovie()
        {
            // Arrange
            var firstMovie = new Movie
            {
                Title = "First Movie To Update",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };

            var SecondMovie = new Movie
            {
                Title = "Second Movie To Update",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { firstMovie, SecondMovie });

            // Modify the movie
            var movieToUpdate = await _dbContext.Movies.Find(x => x.Title == firstMovie.Title).FirstOrDefaultAsync();

            movieToUpdate.Title = "First Movie To Update UPDATED";
            movieToUpdate.Rating = 10;

            // Act
            await _controller.UpdateAsync(movieToUpdate);

            // Assert
            var updatedMovie = await _dbContext.Movies.Find(x => x.Title == movieToUpdate.Title).FirstOrDefaultAsync();
            Assert.NotNull(updatedMovie);
            Assert.Equal(movieToUpdate.Rating, updatedMovie.Rating);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidMovieProvided_ShouldThrowValidationException()
        {
            // Arrange
            // Movie without required fields
            var invalidMovie = new Movie
            {
                Rating = 10,
            };


            // Act and Assert
            Assert.ThrowsAsync<ValidationException>(() => _controller.UpdateAsync(invalidMovie));
        }
    }
}
