using CinemaMS.Controllers;
using CinemaMS.Services.Interfaces;
using Common.CinemaMS.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CinemaMS.Test.Controllers
{
    public class GenreControllerTest
    {
        [Fact]
        public async Task GetMostSuccessfulGenres_ShouldReturnOkResultWithGenres()
        {
            var mockGenreService = new Mock<IGenreService>();
            var genres = new List<GenreDTO>
            {
                new GenreDTO { Id = 1, Name = "Action" },
                new GenreDTO { Id = 2, Name = "Comedy" }
            };

            mockGenreService.Setup(service => service.GetMostSuccessfulGenres())
                .ReturnsAsync(genres);

            var controller = new GenreController(mockGenreService.Object);

            var result = await controller.GetMostSuccessfulGenres();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<IEnumerable<GenreDTO>>(okResult.Value);
            Assert.Equal(genres.Count, returnedGenres.Count());
            Assert.True(genres.SequenceEqual(returnedGenres));
        }

        [Fact]
        public async Task GetMostSuccessfulGenres_ShouldReturnOkEmptyList()
        {
            var mockGenreService = new Mock<IGenreService>();
            mockGenreService.Setup(service => service.GetMostSuccessfulGenres())
                .ReturnsAsync(new List<GenreDTO>());

            var controller = new GenreController(mockGenreService.Object);

            var result = await controller.GetMostSuccessfulGenres();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<IEnumerable<GenreDTO>>(okResult.Value);
            Assert.Empty(returnedGenres);
        }
    }
}
