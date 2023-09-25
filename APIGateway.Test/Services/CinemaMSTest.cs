using APIGateway.Configuration;
using APIGateway.Services;
using Common.CinemaMS.DTO;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace APIGateway.Test.Services
{
    public class CinemaMSTest
    {
        [Fact]
        public async Task GetSuccessfulGenres_ShouldReturnGenres()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var genresResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var genresResponseContent = new List<GenreDTO> 
                    { 
                        new GenreDTO { 
                            Id = 1, Name = "Test" 
                        } 
                    };
           

            genresResponse.Content = new StringContent(JsonConvert.SerializeObject(genresResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(genresResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { CinemaMSURi = "correct_cinema_ms_uri", SuccessufulGenres = "test" });

            var movieService = new CinemaMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            var genres = await movieService.GetSuccessfulGenres();

            Assert.NotNull(genres);
            Assert.Single(genres);

            var genresList = genres.ToList();
            Assert.NotNull(genresList);
            Assert.Contains(genresList, k => k.Id == 1 && k.Name == "Test");
        }

        [Fact]
        public async Task GetSuccessfulGenres_RequestReturnsOKButNullResponse_ShouldReturnEmptyDTO()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var microservicesOptionsMock = new Mock<IOptions<MicroservicesOptions>>();

            var genresResponse = new HttpResponseMessage(HttpStatusCode.OK);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(genresResponse);

            microservicesOptionsMock.Setup(options => options.Value)
                .Returns(new MicroservicesOptions { CinemaMSURi = "correct_cinema_ms_uri", SuccessufulGenres = "test" });

            var movieService = new CinemaMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            var genres = await movieService.GetSuccessfulGenres();

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
                .Returns(new MicroservicesOptions { CinemaMSURi = "WRONG_URI", SuccessufulGenres = "test" });

            var movieService = new CinemaMSService(
                httpClientWrapperMock.Object,
                microservicesOptionsMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetSuccessfulGenres());
        }
    }
}
