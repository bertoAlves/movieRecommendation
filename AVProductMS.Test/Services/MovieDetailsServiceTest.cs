using AVProduct.Configuration;
using AVProduct.DTO;
using AVProduct.Factories.Interfaces;
using AVProduct.Services;
using Common.AVProductMS.DTO;
using Common.Exceptions;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace AVProductMS.Test.Services
{
    public class MovieDetailsServiceTest
    {
        
        [Fact]
        public async Task GetMovieDetails_WrongUri_ShouldThrowHttpRequestException()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieDetailsFactoryMock = new Mock<IMovieDetailsFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();

            var errorApiResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(errorApiResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "WRONG_URI", MovieDetailsEndpoint = "test" });

            var movieService = new MovieDetailsService(
                httpClientWrapperMock.Object,
                movieDetailsFactoryMock.Object,
                movieDbApiOptionsMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetMovieDetails(movieID: 1));
        }

        [Fact]
        public async Task GetMovieDetails_ShouldReturnMovieDetails()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieDetailsFactoryMock = new Mock<IMovieDetailsFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();

            var detailsResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var detailsResponseContent = new ExtDBMovieDetails
            {
                Homepage = "Teste"              
            };

            detailsResponse.Content = new StringContent(JsonConvert.SerializeObject(detailsResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(detailsResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MovieDetailsEndpoint = "test" });

            movieDetailsFactoryMock.Setup(factory => factory.CreateMovieDetailsDTO(It.IsAny<ExtDBMovieDetails>()))
                .Returns((ExtDBMovieDetails details) => new MovieDetailsDTO { Website = details.Homepage });

            var movieService = new MovieDetailsService(
                httpClientWrapperMock.Object,
                movieDetailsFactoryMock.Object,
                movieDbApiOptionsMock.Object);

            var details = await movieService.GetMovieDetails(movieID: 1);

            Assert.NotNull(details);
            Assert.Equal("Teste", details.Website);
        }

        [Fact]
        public async Task GetMovieDetails_RequestReturnsOkButEmpty_ShouldReturnEmptyMovieDetails()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieDetailsFactoryMock = new Mock<IMovieDetailsFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();

            var detailsResponse = new HttpResponseMessage(HttpStatusCode.OK);
            ExtDBMovieDetails detailsResponseContent = null;

            detailsResponse.Content = new StringContent(JsonConvert.SerializeObject(detailsResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(detailsResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MovieDetailsEndpoint = "test" });

            movieDetailsFactoryMock.Setup(factory => factory.CreateMovieDetailsDTO(It.IsAny<ExtDBMovieDetails>()))
                .Returns((ExtDBMovieDetails details) => new MovieDetailsDTO {});

            var movieService = new MovieDetailsService(
                httpClientWrapperMock.Object,
                movieDetailsFactoryMock.Object,
                movieDbApiOptionsMock.Object);

            var details = await movieService.GetMovieDetails(movieID: 1);

            Assert.NotNull(details);
            Assert.Null(details.Website);
        }
        
    }
}
