using AVProduct.Configuration;
using AVProduct.DTO;
using AVProduct.Services;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace AVProductMS.Test.Services
{
    public class KeywordServiceTest
    {

        [Fact]
        public async Task GetMovieKeywords_WrongUri_ShouldThrowHttpRequestException()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();

            var errorApiResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(errorApiResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "WRONG_URI", KeywordsEndpoint = "test" });

            var movieService = new KeywordService(
                httpClientWrapperMock.Object,
                movieDbApiOptionsMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetMovieKeywords(movieID: 1));
        }

        [Fact]
        public async Task GetMovieKeywords_ShouldReturnKeywords()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();

            var keywordApiResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var keywordResponseContent = new ExtDBMovieKeywords
            {
                Keywords = new List<ExtKeywod>
                {
                new ExtKeywod { Id = 1, Name = "Action" },
                new ExtKeywod { Id = 2, Name = "Adventure" }
                }
            };
            keywordApiResponse.Content = new StringContent(JsonConvert.SerializeObject(keywordResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(keywordApiResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", KeywordsEndpoint = "test" });

            var movieService = new KeywordService(
                httpClientWrapperMock.Object,
                movieDbApiOptionsMock.Object);

            var keywords = await movieService.GetMovieKeywords(movieID : 1);

            Assert.NotNull(keywords);
            Assert.Equal(2, keywords.Count());

            var keywordList = keywords.ToList();
            Assert.NotNull(keywordList);
            Assert.Contains(keywordList, k => k.Id == 1 && k.Name == "Action");
            Assert.Contains(keywordList, k => k.Id == 2 && k.Name == "Adventure");
        }

        [Fact]
        public async Task GetMovieKeywords_RequestReturnsOKButEmptyList_ShouldReturnEmptyKeywordList()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();

            var keywordApiResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var keywordResponseContent = new ExtDBMovieKeywords
            {
                Keywords = new List<ExtKeywod>
                {
                }
            };
            keywordApiResponse.Content = new StringContent(JsonConvert.SerializeObject(keywordResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(keywordApiResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", KeywordsEndpoint = "test" });

            var movieService = new KeywordService(
                httpClientWrapperMock.Object,
                movieDbApiOptionsMock.Object);

            var keywords = await movieService.GetMovieKeywords(movieID: 1);

            Assert.NotNull(keywords);
            Assert.Empty(keywords);
        }
    }
}
