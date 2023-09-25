using APIGateway.Configuration;
using APIGateway.Services;
using Common.AVProductMS.DTO;
using Common.CinemaMS.DTO;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Test.Services
{
    public class AVProductMSServiceTest
    {
        [Fact]
        public async Task GetBlockbusterMovies_ShouldReturnMovies()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var date = DateTime.Now;

            var moviesResponseContent = new List<MovieDTO>
            {
                new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Movie Test",
                               Website = "movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "Keyword" } },
                               Overview = "Movie teste description"
                }
            };


            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { AVProductMSUri = "correct_avproduct_ms_uri", BlockbustersMovies = "test" });

            var movieService = new AVProductMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            var movies = await movieService.GetBlockbusterMovies(genres: "27", numberWeeks : 2, bigScreens : 1);

            Assert.NotNull(movies);
            Assert.Single(movies);

            var moviesList = movies.ToList();
            Assert.NotNull(moviesList);
            Assert.Equal("Movie teste description", moviesList[0].Overview);
            Assert.Equal("Movie Test", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetBlockbusterMovies_RequestReturnsOKButNullResponse_ShouldReturnEmptyMovieList()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var genresResponse = new HttpResponseMessage(HttpStatusCode.OK);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(genresResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { AVProductMSUri = "correct_avproduct_ms_uri", BlockbustersMovies = "test" });

            var movieService = new AVProductMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            var genres = await movieService.GetBlockbusterMovies(genres: "27", numberWeeks: 2, bigScreens: 0);

            Assert.NotNull(genres);
            Assert.Empty(genres);
        }

        [Fact]
        public async Task GetSuccessfulGenres_WrongUri_ShouldThrowHttpRequestException()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var genresResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(genresResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { AVProductMSUri = "WRONG_UI", BlockbustersMovies = "test" });

            var movieService = new AVProductMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetBlockbusterMovies(genres: "27", numberWeeks: 2, bigScreens: 1));
        }

        [Fact]
        public async Task GetMinorityGenresMovies_ShouldReturnMovies()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var date = DateTime.Now;

            var moviesResponseContent = new List<MovieDTO>
            {
                new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Movie Test",
                               Website = "movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "Keyword" } },
                               Overview = "Movie teste description"
                }
            };


            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { AVProductMSUri = "correct_avproduct_ms_uri", MinorityGenresMovies = "test" });

            var movieService = new AVProductMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            var movies = await movieService.GetMinorityGenresMovies(without_genreIds: "27", numberWeeks: 2, smallScreens: 1);

            Assert.NotNull(movies);
            Assert.Single(movies);

            var moviesList = movies.ToList();
            Assert.NotNull(moviesList);
            Assert.Equal("Movie teste description", moviesList[0].Overview);
            Assert.Equal("Movie Test", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_RequestReturnsOKButNullResponse_ShouldReturnEmptyMovieList()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var genresResponse = new HttpResponseMessage(HttpStatusCode.OK);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(genresResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { AVProductMSUri = "correct_avproduct_ms_uri", MinorityGenresMovies = "test" });

            var movieService = new AVProductMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            var genres = await movieService.GetMinorityGenresMovies(without_genreIds: "27", numberWeeks: 2, smallScreens: 0);

            Assert.NotNull(genres);
            Assert.Empty(genres);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_WrongUri_ShouldThrowHttpRequestException()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var genresResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(genresResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { AVProductMSUri = "WRONG_UI", MinorityGenresMovies = "test" });

            var movieService = new AVProductMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetMinorityGenresMovies(without_genreIds: "27", numberWeeks: 2, smallScreens: 1));
        }
    }
}
