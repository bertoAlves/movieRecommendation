using CinemaMS.Configuration;
using CinemaMS.DAL.Interfaces;
using CinemaMS.Factories.Interfaces;
using CinemaMS.Models;
using CinemaMS.Services;
using Common.CinemaMS.DTO;
using Common.Exceptions;
using Microsoft.Extensions.Options;
using Moq;

namespace CinemaMS.Test.Services
{
    public class GenreServiceTest
    {
        [Fact]
        public async Task GetMostSuccessfulGenres_ShouldReturnGenres()
        {
            var genreDALMock = new Mock<IGenreDAL>();
            var factoryMock = new Mock<IBuildGenreDTO>();
            var optionsMock = new Mock<IOptions<SuccessfulGenresOptions>>();

            var genresToReturn = new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" }
            };

            genreDALMock.Setup(dal => dal.GetMostSuccessfulGenres(It.IsAny<int>()))
                .ReturnsAsync(genresToReturn);

            var genreDTOsToReturn = genresToReturn.Select(genre => new GenreDTO { Id = genre.Id, Name = genre.Name });

            factoryMock.Setup(factory => factory.CreateGenreDTO(It.IsAny<Genre>()))
                .Returns((Genre genre) => new GenreDTO { Id = genre.Id, Name = genre.Name });

            optionsMock.Setup(options => options.Value)
                .Returns(new SuccessfulGenresOptions { Algorithm = "SELL-OUTS", MinNumberSellouts = 2 });

            var genreService = new GenreService(genreDALMock.Object, factoryMock.Object, optionsMock.Object);

            var result = await genreService.GetMostSuccessfulGenres();

            Assert.NotNull(result);
            Assert.True(genreDTOsToReturn.SequenceEqual(result));
        }

        [Fact]
        public async Task GetMostSuccessfulGenres_NoSuccessfulGenresOptions_ShouldReturnGenres()
        {
            var genreDALMock = new Mock<IGenreDAL>();
            var factoryMock = new Mock<IBuildGenreDTO>();
            var optionsMock = new Mock<IOptions<SuccessfulGenresOptions>>();

            var genresToReturn = new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" }
            };

            genreDALMock.Setup(dal => dal.GetMostSuccessfulGenres(It.IsAny<int>()))
                .ReturnsAsync(genresToReturn);

            var genreDTOsToReturn = genresToReturn.Select(genre => new GenreDTO { Id = genre.Id, Name = genre.Name });

            factoryMock.Setup(factory => factory.CreateGenreDTO(It.IsAny<Genre>()))
                .Returns((Genre genre) => new GenreDTO { Id = genre.Id, Name = genre.Name });

            optionsMock.Setup(options => options.Value)
                .Returns(new SuccessfulGenresOptions { /* EMPTY */ });

            var genreService = new GenreService(genreDALMock.Object, factoryMock.Object, optionsMock.Object);

            var result = await genreService.GetMostSuccessfulGenres();

            Assert.NotNull(result);
            Assert.True(genreDTOsToReturn.SequenceEqual(result));
        }


        [Fact]
        public async Task GetMostSuccessfulGenres_EmptyListGenres_ShouldReturnEmptyGenreList()
        {
            var genreDALMock = new Mock<IGenreDAL>();
            var factoryMock = new Mock<IBuildGenreDTO>();
            var optionsMock = new Mock<IOptions<SuccessfulGenresOptions>>();

            var genresToReturn = new List<Genre>
            {
            };

            genreDALMock.Setup(dal => dal.GetMostSuccessfulGenres(It.IsAny<int>()))
                .ReturnsAsync(genresToReturn);

            var genreDTOsToReturn = genresToReturn.Select(genre => new GenreDTO { Id = genre.Id, Name = genre.Name });

            factoryMock.Setup(factory => factory.CreateGenreDTO(It.IsAny<Genre>()))
                .Returns((Genre genre) => new GenreDTO { Id = genre.Id, Name = genre.Name });

            optionsMock.Setup(options => options.Value)
                .Returns(new SuccessfulGenresOptions { Algorithm = "SELL-OUTS", MinNumberSellouts = 0 });

            var genreService = new GenreService(genreDALMock.Object, factoryMock.Object, optionsMock.Object);

            var result = await genreService.GetMostSuccessfulGenres();

            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
        }


        [Fact]
        public async Task GetMostSuccessfulGenres_UnexistingAlgorithm_ShouldThrowNotImplementedException()
        {
            var genreDALMock = new Mock<IGenreDAL>();
            var factoryMock = new Mock<IBuildGenreDTO>();
            var optionsMock = new Mock<IOptions<SuccessfulGenresOptions>>();

            optionsMock.Setup(options => options.Value)
                .Returns(new SuccessfulGenresOptions { Algorithm = "UNEXISTING_ALGORITHM", MinNumberSellouts = 0 });

            var genreService = new GenreService(genreDALMock.Object, factoryMock.Object, optionsMock.Object);

            await Assert.ThrowsAsync<UnsupportedAlgorithmException>(async () => await genreService.GetMostSuccessfulGenres());
        }
    }
}
