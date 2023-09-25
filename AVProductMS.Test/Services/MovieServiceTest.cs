using AVProduct.Configuration;
using AVProduct.DTO;
using AVProduct.Factories.Interfaces;
using AVProduct.Services;
using AVProduct.Services.Interfaces;
using Common.AVProductMS.DTO;
using Common.Exceptions;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AVProductMS.Test.Services
{
    public class MovieServiceTest
    {
        #region GetBlockbusters
        [Fact]
        public async Task GetBlockbusterMovies_WrongUri_ShouldThrowHttpRequestException()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                          .Returns(false);

            var errorApiResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(errorApiResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "WRONG_URI", BlockbustersEndpoint = "test" });

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetBlockbusterMovies(genres: "27,28", numberOfWeeks: 2, numberOfBigScreens: 2));
        }


        [Fact]
        public async Task GetBlockbusterMovies_ShouldReturnMovies()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                        .Returns(false);

            memoryCacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                        .Returns(Mock.Of<ICacheEntry>());

            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var moviesResponseContent = new MovieDBResponse
            {
                Results = new List<ExtDBMovie>() {
                    new ExtDBMovie {
                        ID = 1,
                        Title = "Teste",
                        Overview = "Teste Overview",
                        ReleaseDate = date,
                        OriginalLanguage = "en",
                        GenresIDs = new List<int> { 1, 2, }
                    }
                }
            };

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { Id = details.ID, Title = details.Title, Overview = details.Overview, ReleaseDate = details.ReleaseDate, Language = details.OriginalLanguage, Genres = details.GenresIDs });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { Website = "test" });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } });

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetBlockbusterMovies(genres: "27", numberOfWeeks: 2, numberOfBigScreens: 2);

            Assert.NotNull(movies);
            Assert.NotEmpty(movies);

            var moviesList = movies.ToList();
            Assert.Equal("test", moviesList[0].Website);
            Assert.Equal("Teste Overview", moviesList[0].Overview);
            Assert.Equal("Teste", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetBlockbusterMovies_RequestReturnsOKButReturnsResultsAndNoKeywords_ShouldReturnMoviesWithNoKeywords()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                        .Returns(false);

            memoryCacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                        .Returns(Mock.Of<ICacheEntry>());


            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var moviesResponseContent = new MovieDBResponse
            {
                Results = new List<ExtDBMovie>() {
                    new ExtDBMovie {
                        ID = 1,
                        Title = "Teste",
                        Overview = "Teste Overview",
                        ReleaseDate = date,
                        OriginalLanguage = "en",
                        GenresIDs = new List<int> { 1, 2, }
                    }
                }
            };

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { Id = details.ID, Title = details.Title, Overview = details.Overview, ReleaseDate = details.ReleaseDate, Language = details.OriginalLanguage, Genres = details.GenresIDs });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { Website = "test" });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { /*Empty*/ });

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetBlockbusterMovies(genres: "27", numberOfWeeks: 2, numberOfBigScreens: 2);

            Assert.NotNull(movies);
            Assert.NotEmpty(movies);

            var moviesList = movies.ToList();
            Assert.Equal("test", moviesList[0].Website);
            Assert.Equal("Teste Overview", moviesList[0].Overview);
            Assert.Equal("Teste", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetBlockbusterMovies_RequestReturnsOKButNoDetailsNoKeywords_ShouldReturnMoviesWithNoDetails()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                        .Returns(false);

            memoryCacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                        .Returns(Mock.Of<ICacheEntry>());


            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var moviesResponseContent = new MovieDBResponse
            {
                Results = new List<ExtDBMovie>() {
                    new ExtDBMovie {
                        ID = 1,
                        Title = "Teste",
                        Overview = "Teste Overview",
                        ReleaseDate = date,
                        OriginalLanguage = "en",
                        GenresIDs = new List<int> { 1, 2, }
                    }
                }
            };

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { Id = details.ID, Title = details.Title, Overview = details.Overview, ReleaseDate = details.ReleaseDate, Language = details.OriginalLanguage, Genres = details.GenresIDs });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { /*Empty*/ });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { /*Empty*/});

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetBlockbusterMovies(genres: "27", numberOfWeeks: 2, numberOfBigScreens: 2);

            Assert.NotNull(movies);
            Assert.NotEmpty(movies);

            var moviesList = movies.ToList();
            Assert.Null(moviesList[0].Website);
            Assert.Equal("Teste Overview", moviesList[0].Overview);
            Assert.Equal("Teste", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetBlockbusterMovies_RequestReturnsOKButReturnsNull_ShouldReturnEmptyMovieList()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                          .Returns(false);


            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            MovieDBResponse moviesResponseContent = null;

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { /*Empty*/ });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { /*Empty*/});

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetBlockbusterMovies(genres: "27", numberOfWeeks: 2, numberOfBigScreens: 2);

            Assert.NotNull(movies);
            Assert.Empty(movies);
        }
        #endregion

        #region MinorityGenres
        [Fact]
            public async Task GetMinorityGenresMovies_WrongUri_ShouldThrowHttpRequestException()
            {
                var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
                var movieFactoryMock = new Mock<IMovieFactory>();
                var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
                var movieDetailsService = new Mock<IMovieDetailsService>();
                var keywordService = new Mock<IKeywordService>();
                var memoryCacheMock = new Mock<IMemoryCache>();

                memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                              .Returns(false);

                var errorApiResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

                httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                    .ReturnsAsync(errorApiResponse);

                movieDbApiOptionsMock.Setup(options => options.Value)
                    .Returns(new MovieDBApiOptions { BaseUri = "WRONG_URI", MinorityGenresEndpoint = "test" });

                var movieService = new MovieService(
                    httpClientWrapperMock.Object,
                    movieFactoryMock.Object,
                    movieDbApiOptionsMock.Object,
                    keywordService.Object,
                    movieDetailsService.Object,
                    memoryCacheMock.Object);

                await Assert.ThrowsAsync<HttpRequestException>(() => movieService.GetMinorityGenresMovies(withoutgenres : "27,28", numberOfWeeks: 2, numberOfSmallScreens: 2));
            }

            [Fact]
            public async Task GetMinorityGenresMovies_ShouldReturnMovies()
            {
                var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
                var movieFactoryMock = new Mock<IMovieFactory>();
                var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
                var movieDetailsService = new Mock<IMovieDetailsService>();
                var keywordService = new Mock<IKeywordService>();
                var memoryCacheMock = new Mock<IMemoryCache>();

                memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                        .Returns(false);

                memoryCacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                        .Returns(Mock.Of<ICacheEntry>());

                var date = DateTime.Now;
                    var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    var moviesResponseContent = new MovieDBResponse { Results = new List<ExtDBMovie>() { 
                        new ExtDBMovie {
                            ID = 1,
                            Title = "Teste",
                            Overview = "Teste Overview",
                            ReleaseDate = date,
                            OriginalLanguage = "en",
                            GenresIDs = new List<int> { 1, 2, }
                        }
                    } 
                    };

                moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

                httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                    .ReturnsAsync(moviesResponse);

                movieDbApiOptionsMock.Setup(options => options.Value)
                    .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

                movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                    .Returns((ExtDBMovie details) => new MovieDTO { Id = details.ID, Title = details.Title, Overview = details.Overview, ReleaseDate = details.ReleaseDate, Language = details.OriginalLanguage, Genres = details.GenresIDs });

                movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                    .ReturnsAsync(new MovieDetailsDTO { Website = "test" });

                keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                    .ReturnsAsync(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } });

                var movieService = new MovieService(
                    httpClientWrapperMock.Object,
                    movieFactoryMock.Object,
                    movieDbApiOptionsMock.Object,
                    keywordService.Object,
                    movieDetailsService.Object,
                    memoryCacheMock.Object);

                var movies = await movieService.GetMinorityGenresMovies(withoutgenres : "27", numberOfWeeks : 2, numberOfSmallScreens : 2);

                Assert.NotNull(movies);
                Assert.NotEmpty(movies);

                var moviesList = movies.ToList();
                Assert.Equal("test", moviesList[0].Website);
                Assert.Equal("Teste Overview", moviesList[0].Overview);
                Assert.Equal("Teste", moviesList[0].Title);
                Assert.Equal("en", moviesList[0].Language);
                Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
                Assert.Equal(date, moviesList[0].ReleaseDate);
                Assert.Equal(1, moviesList[0].Id);
                Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_RequestReturnsOKButReturnsDetailsAndNoKeywords_ShouldReturnMoviesWithNoKeywords()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                    .Returns(false);

            memoryCacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                    .Returns(Mock.Of<ICacheEntry>());

            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var moviesResponseContent = new MovieDBResponse
            {
                Results = new List<ExtDBMovie>() {
                    new ExtDBMovie {
                        ID = 1,
                        Title = "Teste",
                        Overview = "Teste Overview",
                        ReleaseDate = date,
                        OriginalLanguage = "en",
                        GenresIDs = new List<int> { 1, 2, }
                    }
                }
            };

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { Id = details.ID, Title = details.Title, Overview = details.Overview, ReleaseDate = details.ReleaseDate, Language = details.OriginalLanguage, Genres = details.GenresIDs });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { Website = "test" });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { /*Empty*/ });

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetMinorityGenresMovies(withoutgenres: "27", numberOfWeeks: 2, numberOfSmallScreens: 2);

            Assert.NotNull(movies);
            Assert.NotEmpty(movies);

            var moviesList = movies.ToList();
            Assert.Equal("test", moviesList[0].Website);
            Assert.Equal("Teste Overview", moviesList[0].Overview);
            Assert.Equal("Teste", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_RequestReturnsOKButReturnsNoDetailsAndNoKeywords_ShouldReturnMoviesWithNoDetails()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                          .Returns(false);

            memoryCacheMock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                    .Returns(Mock.Of<ICacheEntry>());

            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var moviesResponseContent = new MovieDBResponse
            {
                Results = new List<ExtDBMovie>() {
                    new ExtDBMovie {
                        ID = 1,
                        Title = "Teste",
                        Overview = "Teste Overview",
                        ReleaseDate = date,
                        OriginalLanguage = "en",
                        GenresIDs = new List<int> { 1, 2, }
                    }
                }
            };

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { Id = details.ID, Title = details.Title, Overview = details.Overview, ReleaseDate = details.ReleaseDate, Language = details.OriginalLanguage, Genres = details.GenresIDs });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { /*Empty*/ });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { /*Empty*/});

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetMinorityGenresMovies(withoutgenres: "27", numberOfWeeks: 2, numberOfSmallScreens: 2);

            Assert.NotNull(movies);
            Assert.NotEmpty(movies);

            var moviesList = movies.ToList();
            Assert.Null(moviesList[0].Website);
            Assert.Equal("Teste Overview", moviesList[0].Overview);
            Assert.Equal("Teste", moviesList[0].Title);
            Assert.Equal("en", moviesList[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, moviesList[0].Genres);
            Assert.Equal(date, moviesList[0].ReleaseDate);
            Assert.Equal(1, moviesList[0].Id);
            Assert.Equal(new List<KeywordDTO> { }, moviesList[0].Keywords);
        }

        [Fact]
        public async Task GetMinorityGenresMovies_RequestReturnsOKButReturnsNull_ShouldReturnEmptyMovieList()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var movieFactoryMock = new Mock<IMovieFactory>();
            var movieDbApiOptionsMock = new Mock<IOptions<MovieDBApiOptions>>();
            var movieDetailsService = new Mock<IMovieDetailsService>();
            var keywordService = new Mock<IKeywordService>();
            var memoryCacheMock = new Mock<IMemoryCache>();

            memoryCacheMock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
                          .Returns(false);

            var date = DateTime.Now;
            var moviesResponse = new HttpResponseMessage(HttpStatusCode.OK);
            MovieDBResponse moviesResponseContent = null;

            moviesResponse.Content = new StringContent(JsonConvert.SerializeObject(moviesResponseContent));

            httpClientWrapperMock.Setup(factory => factory.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(moviesResponse);

            movieDbApiOptionsMock.Setup(options => options.Value)
                .Returns(new MovieDBApiOptions { BaseUri = "correct_movie_db_uri", MinorityGenresEndpoint = "test" });

            movieFactoryMock.Setup(factory => factory.CreateMovieDTO(It.IsAny<ExtDBMovie>()))
                .Returns((ExtDBMovie details) => new MovieDTO { });

            movieDetailsService.Setup(movieDetails => movieDetails.GetMovieDetails(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsDTO { /*Empty*/ });

            keywordService.Setup(movieDetails => movieDetails.GetMovieKeywords(It.IsAny<int>()))
                .ReturnsAsync(new List<KeywordDTO> { /*Empty*/});

            var movieService = new MovieService(
                httpClientWrapperMock.Object,
                movieFactoryMock.Object,
                movieDbApiOptionsMock.Object,
                keywordService.Object,
                movieDetailsService.Object,
                memoryCacheMock.Object);

            var movies = await movieService.GetMinorityGenresMovies(withoutgenres: "27", numberOfWeeks: 2, numberOfSmallScreens: 2);

            Assert.NotNull(movies);
            Assert.Empty(movies);
        }

        #endregion

    }
}
