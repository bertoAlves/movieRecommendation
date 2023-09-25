using APIGateway.Configuration;
using APIGateway.Factories.Interfaces;
using APIGateway.Services;
using APIGateway.Services.Interfaces;
using CinemaMS.Services;
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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace APIGateway.Test.Services
{
    public class RecommendationServiceTest
    {
        [Fact]
        public async Task GetIntelligentBillboard_ShouldReturnBillboard()
        {
            var cinemaService = new Mock<ICinemaMSService>();
            var avproductService = new Mock<IAVProductMSService>();
            var factoryMock = new Mock<IGenreFactory>();
            var date = DateTime.Now;

            cinemaService.Setup(cinemas => cinemas.GetSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO> { new GenreDTO { Id = 1, Name = "Action" } });

            avproductService.Setup(movieDetails => movieDetails.GetBlockbusterMovies(It.IsAny<string>(), It.IsAny<int>(),It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Blockbuster Movie Test",
                               Website = "Blockbuster movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "Keyword" } },
                               Overview = "Blockbuster Movie teste description"
                } });

            avproductService.Setup(movieDetails => movieDetails.GetMinorityGenresMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 2,
                               Language = "en",
                               Title = "Minority Movie Test",
                               Website = "Minority movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 3, 4},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 2, Name = "Minority Keyword" } },
                               Overview = "Minority Movie teste description"
                } });

            var recommendationService = new RecommendationService(
                cinemaService.Object,
                factoryMock.Object,
                avproductService.Object);

            var billboard = await recommendationService.GetIntelligentBillboard(numberWeeks : 1, bigScreens : 1, smallScreens:1, useSuccessfulGenres: true);

            Assert.NotNull(billboard);
            Assert.NotNull(billboard.billboard);
            Assert.NotNull(billboard.billboard[0]);
            Assert.NotNull(billboard.billboard[0].BigScreens);
            Assert.NotEmpty(billboard.billboard[0].BigScreens);
            Assert.NotNull(billboard.billboard[0].SmallScreens);
            Assert.NotEmpty(billboard.billboard[0].SmallScreens);

            var bigscreenMovies = billboard.billboard[0].BigScreens.ToList();
            var smallScreenMovies = billboard.billboard[0].SmallScreens.ToList();

            Assert.Equal("Blockbuster Movie teste description", bigscreenMovies[0].Overview);
            Assert.Equal("Blockbuster Movie Test", bigscreenMovies[0].Title);
            Assert.Equal("en", bigscreenMovies[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, bigscreenMovies[0].Genres);
            Assert.Equal(date, bigscreenMovies[0].ReleaseDate);
            Assert.Equal(1, bigscreenMovies[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } }, bigscreenMovies[0].Keywords);

            Assert.Equal("Minority Movie teste description", smallScreenMovies[0].Overview);
            Assert.Equal("Minority Movie Test", smallScreenMovies[0].Title);
            Assert.Equal("en", smallScreenMovies[0].Language);
            Assert.Equal(new List<int>() { 3, 4 }, smallScreenMovies[0].Genres);
            Assert.Equal(date, smallScreenMovies[0].ReleaseDate);
            Assert.Equal(2, smallScreenMovies[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 2, Name = "Minority Keyword" } }, smallScreenMovies[0].Keywords);

        }

        [Fact]
        public async Task GetIntelligentBillboard_ShouldReturnBillboardOnlyWithBlockbusters()
        {
            var cinemaService = new Mock<ICinemaMSService>();
            var avproductService = new Mock<IAVProductMSService>();
            var factoryMock = new Mock<IGenreFactory>();
            var date = DateTime.Now;

            cinemaService.Setup(cinemas => cinemas.GetSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO> { new GenreDTO { Id = 1, Name = "Action" } });

            avproductService.Setup(movieDetails => movieDetails.GetBlockbusterMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Blockbuster Movie Test",
                               Website = "Blockbuster movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "Keyword" } },
                               Overview = "Blockbuster Movie teste description"
                } });

            avproductService.Setup(movieDetails => movieDetails.GetMinorityGenresMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> {  });

            var recommendationService = new RecommendationService(
                cinemaService.Object,
                factoryMock.Object,
                avproductService.Object);

            var billboard = await recommendationService.GetIntelligentBillboard(numberWeeks: 1, bigScreens: 1, smallScreens: 1, useSuccessfulGenres: true);

            Assert.NotNull(billboard);
            Assert.NotNull(billboard.billboard);
            Assert.NotNull(billboard.billboard[0]);
            Assert.NotNull(billboard.billboard[0].BigScreens);
            Assert.NotEmpty(billboard.billboard[0].BigScreens);
            Assert.NotNull(billboard.billboard[0].SmallScreens);
            Assert.Empty(billboard.billboard[0].SmallScreens);

            var bigscreenMovies = billboard.billboard[0].BigScreens.ToList();

            Assert.Equal("Blockbuster Movie teste description", bigscreenMovies[0].Overview);
            Assert.Equal("Blockbuster Movie Test", bigscreenMovies[0].Title);
            Assert.Equal("en", bigscreenMovies[0].Language);
            Assert.Equal(new List<int> { 1, 2, }, bigscreenMovies[0].Genres);
            Assert.Equal(date, bigscreenMovies[0].ReleaseDate);
            Assert.Equal(1, bigscreenMovies[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 1, Name = "Keyword" } }, bigscreenMovies[0].Keywords);
        }

        [Fact]
        public async Task GetIntelligentBillboard_ShouldReturnBillboardOnlyWithMinorityGenres()
        {
            var cinemaService = new Mock<ICinemaMSService>();
            var avproductService = new Mock<IAVProductMSService>();
            var factoryMock = new Mock<IGenreFactory>();
            var date = DateTime.Now;

            cinemaService.Setup(cinemas => cinemas.GetSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO> { new GenreDTO { Id = 1, Name = "Action" } });

            avproductService.Setup(movieDetails => movieDetails.GetBlockbusterMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { });

            avproductService.Setup(movieDetails => movieDetails.GetMinorityGenresMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 2,
                               Language = "en",
                               Title = "Minority Movie Test",
                               Website = "Minority movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 3, 4},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 2, Name = "Minority Keyword" } },
                               Overview = "Minority Movie teste description"
                } });

            var recommendationService = new RecommendationService(
                cinemaService.Object,
                factoryMock.Object,
                avproductService.Object);

            var billboard = await recommendationService.GetIntelligentBillboard(numberWeeks: 1, bigScreens: 1, smallScreens: 1, useSuccessfulGenres: true);

            Assert.NotNull(billboard);
            Assert.NotNull(billboard.billboard);
            Assert.NotNull(billboard.billboard[0]);
            Assert.NotNull(billboard.billboard[0].BigScreens);
            Assert.Empty(billboard.billboard[0].BigScreens);
            Assert.NotNull(billboard.billboard[0].SmallScreens);
            Assert.NotEmpty(billboard.billboard[0].SmallScreens);

            var smallScreenMovies = billboard.billboard[0].SmallScreens.ToList();

            Assert.Equal("Minority Movie teste description", smallScreenMovies[0].Overview);
            Assert.Equal("Minority Movie Test", smallScreenMovies[0].Title);
            Assert.Equal("en", smallScreenMovies[0].Language);
            Assert.Equal(new List<int>() { 3, 4 }, smallScreenMovies[0].Genres);
            Assert.Equal(date, smallScreenMovies[0].ReleaseDate);
            Assert.Equal(2, smallScreenMovies[0].Id);
            Assert.Equal(new List<KeywordDTO> { new KeywordDTO { Id = 2, Name = "Minority Keyword" } }, smallScreenMovies[0].Keywords);
        }

        [Fact]
        public async Task GetIntelligentBillboard_NoMovieReturn_ShouldReturnEmptyBillboard()
        {
            var cinemaService = new Mock<ICinemaMSService>();
            var avproductService = new Mock<IAVProductMSService>();
            var factoryMock = new Mock<IGenreFactory>();
            var date = DateTime.Now;

            cinemaService.Setup(cinemas => cinemas.GetSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO> { new GenreDTO { Id = 1, Name = "Action" } });

            avproductService.Setup(movieDetails => movieDetails.GetBlockbusterMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { });

            avproductService.Setup(movieDetails => movieDetails.GetMinorityGenresMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { });

            var recommendationService = new RecommendationService(
                cinemaService.Object,
                factoryMock.Object,
                avproductService.Object);

            var billboard = await recommendationService.GetIntelligentBillboard(numberWeeks: 1, bigScreens: 1, smallScreens: 1, useSuccessfulGenres: true);

            Assert.NotNull(billboard);
            Assert.Empty(billboard.billboard);
        }

        [Fact]
        public async Task GetIntelligentBillboard_ZeroWeeks_ShouldReturnEmptyBillboard()
        {
            var cinemaService = new Mock<ICinemaMSService>();
            var avproductService = new Mock<IAVProductMSService>();
            var factoryMock = new Mock<IGenreFactory>();
            var date = DateTime.Now;

            cinemaService.Setup(cinemas => cinemas.GetSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO> { new GenreDTO { Id = 1, Name = "Action" } });

            avproductService.Setup(movieDetails => movieDetails.GetBlockbusterMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Blockbuster Movie Test",
                               Website = "Blockbuster movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "Keyword" } },
                               Overview = "Blockbuster Movie teste description"
                } });

            avproductService.Setup(movieDetails => movieDetails.GetMinorityGenresMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 2,
                               Language = "en",
                               Title = "Minority Movie Test",
                               Website = "Minority movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 3, 4},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 2, Name = "Minority Keyword" } },
                               Overview = "Minority Movie teste description"
                } });

            var recommendationService = new RecommendationService(
                cinemaService.Object,
                factoryMock.Object,
                avproductService.Object);

            var billboard = await recommendationService.GetIntelligentBillboard(numberWeeks: 0, bigScreens: 1, smallScreens: 1, useSuccessfulGenres: true);

            Assert.NotNull(billboard);
            Assert.Empty(billboard.billboard);
        }

        [Fact]
        public async Task GetIntelligentBillboard_ZeroBigScreensAndSmallScreens_ShouldReturnEmptyBillboard()
        {
            var cinemaService = new Mock<ICinemaMSService>();
            var avproductService = new Mock<IAVProductMSService>();
            var factoryMock = new Mock<IGenreFactory>();
            var date = DateTime.Now;

            cinemaService.Setup(cinemas => cinemas.GetSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO> { new GenreDTO { Id = 1, Name = "Action" } });

            avproductService.Setup(movieDetails => movieDetails.GetBlockbusterMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 1,
                               Language = "en",
                               Title = "Blockbuster Movie Test",
                               Website = "Blockbuster movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 1, 2},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 1, Name = "Keyword" } },
                               Overview = "Blockbuster Movie teste description"
                } });

            avproductService.Setup(movieDetails => movieDetails.GetMinorityGenresMovies(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<MovieDTO> { new MovieDTO { Id = 2,
                               Language = "en",
                               Title = "Minority Movie Test",
                               Website = "Minority movie homepage",
                               ReleaseDate = date,
                               Genres = new List<int>(){ 3, 4},
                               Keywords = new List<KeywordDTO>(){ new KeywordDTO { Id = 2, Name = "Minority Keyword" } },
                               Overview = "Minority Movie teste description"
                } });

            var recommendationService = new RecommendationService(
                cinemaService.Object,
                factoryMock.Object,
                avproductService.Object);

            var billboard = await recommendationService.GetIntelligentBillboard(numberWeeks: 1, bigScreens: 0, smallScreens: 0, useSuccessfulGenres: true);

            Assert.NotNull(billboard);
            Assert.Empty(billboard.billboard);
        }
    }
}
