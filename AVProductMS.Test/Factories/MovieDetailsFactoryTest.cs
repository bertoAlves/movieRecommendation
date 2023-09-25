using AVProduct.DTO;
using AVProduct.Factories;

namespace AVProductMS.Test.Factories
{
    public class MovieDetailsFactoryTest
    {
        [Fact]
        public void CreateMovieDetailsDTO_ShouldCreateMovieDetailsDTO()
        {
            var factory = new MovieDetailsFactory();
            var dbMovie = new ExtDBMovieDetails { Homepage = "website" };

            var result = factory.CreateMovieDetailsDTO(dbMovie);

            Assert.NotNull(result);
            Assert.Equal("website", result.Website);
        }

        [Fact]
        public void CreateMovieDetailsDTO_NullExtDBMovieDetails_ShouldCreateEmptyMovieDetailsDTO()
        {
            var factory = new MovieDetailsFactory();
            ExtDBMovieDetails dbMovie = null;

            var result = factory.CreateMovieDetailsDTO(dbMovie);

            Assert.NotNull(result);
            Assert.Null(result.Website);
        }
    }
}
