using AVProduct.Controllers;
using AVProduct.Services.Interfaces;
using Common.AVProductMS.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AVProductMS.Test.Controllers
{
    public class MovieControllerTest
    {
        [Fact]
        public async Task GetAllTimeMovies_ShouldReturnOkResultWithMovies()
        {
            var mockMovieService = new Mock<IMovieService>();
            var movies = new List<MovieDTO>
            {
                new MovieDTO { Id = 1, 
                               Language = "en", 
                               Title = "Movie Test", 
                               Website = "movie homepage", 
                               ReleaseDate = DateTime.Now, 
                               Genres = new List<int>(){ 1, 2}, 
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "keyword" } },
                               Overview = "Movie teste description"
                }
            };

            mockMovieService.Setup(service => service.GetAllTimeMovies("1","1,2"))
                .ReturnsAsync(movies);

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetAllTimeMovies("1", "1,2");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMovies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Equal(movies.Count, returnedMovies.Count());
            Assert.True(movies.SequenceEqual(returnedMovies));
        }

        [Fact]
        public async Task GetAllTimeMovies_ShouldReturnOkEmptyList()
        {
            var mockMovieService = new Mock<IMovieService>();

            mockMovieService.Setup(service => service.GetAllTimeMovies("1", "1,2"))
                .ReturnsAsync(new List<MovieDTO>());

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetAllTimeMovies("1", "1,2");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Empty(returnedGenres);
        }

        [Fact]
        public async Task GetBlockbusters_ShouldReturnOkResultWithMovies()
        {
            var mockMovieService = new Mock<IMovieService>();
            var movies = new List<MovieDTO>
            {
                new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Movie Test",
                               Website = "movie homepage",
                               ReleaseDate = DateTime.Now,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "keyword" } },
                               Overview = "Movie teste description"
                }
            };

            mockMovieService.Setup(service => service.GetBlockbusterMovies("1", 1, 2))
                .ReturnsAsync(movies);

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetBlockbusterMovies(genres: "1", numberOfWeeks:1, numberOfBigScreens:2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMovies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Equal(movies.Count, returnedMovies.Count());
            Assert.True(movies.SequenceEqual(returnedMovies));
        }

        [Fact]
        public async Task GetBlockbusters_ShouldReturnOkEmptyList()
        {
            var mockMovieService = new Mock<IMovieService>();

            mockMovieService.Setup(service => service.GetBlockbusterMovies("1", 0, 2))
                .ReturnsAsync(new List<MovieDTO>());

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetBlockbusterMovies(genres: "1", numberOfWeeks: 1, numberOfBigScreens: 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Empty(returnedGenres);
        }

        [Fact]
        public async Task GetBlockbusters_ZeroNumberOfWeeks_ShouldThrowBadRequest()
        {
            var mockMovieService = new Mock<IMovieService>();

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetBlockbusterMovies(genres:"1", numberOfWeeks:0, numberOfBigScreens:2);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The 'numberOfWeeks' parameter must be a positive number.", badRequest.Value);
        }

        [Fact]
        public async Task GetBlockbusters_ZeroNumberOfBigScreens_ShouldThrowBadRequest()
        {
            var mockMovieService = new Mock<IMovieService>();

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetBlockbusterMovies(genres: "1", numberOfWeeks: 2, numberOfBigScreens: 0);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The 'numberOfBigScreens' parameter must be a positive number.", badRequest.Value);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_ShouldReturnOkResultWithMovies()
        {
            var mockMovieService = new Mock<IMovieService>();
            var movies = new List<MovieDTO>
            {
                new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Movie Test",
                               Website = "movie homepage",
                               ReleaseDate = DateTime.Now,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "keyword" } },
                               Overview = "Movie teste description"
                }
            };

            mockMovieService.Setup(service => service.GetMinorityGenresMovies("1", 1, 2))
                .ReturnsAsync(movies);

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetMinorityGenresMovies(withoutgenres: "1", numberOfWeeks: 1, numberOfSmallScreens: 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMovies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Equal(movies.Count, returnedMovies.Count());
            Assert.True(movies.SequenceEqual(returnedMovies));
        }

        [Fact]
        public async Task GetMinorityGenresMovies_ShouldReturnOkEmptyList()
        {
            var mockMovieService = new Mock<IMovieService>();

            mockMovieService.Setup(service => service.GetMinorityGenresMovies("1", 1, 2))
                .ReturnsAsync(new List<MovieDTO>());

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetMinorityGenresMovies(withoutgenres: "1", numberOfWeeks: 1, numberOfSmallScreens: 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
            Assert.Empty(returnedGenres);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_ZeroNumberOfWeeks_ShouldThrowBadRequest()
        {
            var mockMovieService = new Mock<IMovieService>();

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetMinorityGenresMovies(withoutgenres: "1", numberOfWeeks: 0, numberOfSmallScreens: 2);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The 'numberOfWeeks' parameter must be a positive number.", badRequest.Value);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_ZeroNumberOfSmallScreens_ShouldThrowBadRequest()
        {
            var mockMovieService = new Mock<IMovieService>();

            var controller = new MovieController(mockMovieService.Object);

            var result = await controller.GetMinorityGenresMovies(withoutgenres: "1", numberOfWeeks: 2, numberOfSmallScreens: 0);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The 'numberOfSmallScreens' parameter must be a positive number.", badRequest.Value);
        }

    }
}