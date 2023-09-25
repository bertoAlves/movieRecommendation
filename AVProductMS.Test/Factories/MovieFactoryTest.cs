using AVProduct.DTO;
using AVProduct.Factories;

namespace AVProductMS.Test.Factories
{
    public class MovieFactoryTest
    {
        [Fact]
        public void CreateMovieDTO_ShouldCreateMoviesDTO()
        {
            var factory = new MovieFactory();

            DateTime timestamp = DateTime.Now;
            List<int> genres = new List<int>() { 1, 2 };

            var dbMovie = new ExtDBMovie { ID = 1, Title = "Teste", ReleaseDate = timestamp, Overview = "Teste Overview",  OriginalLanguage = "en",  GenresIDs = genres };

            var result = factory.CreateMovieDTO(dbMovie);

            Assert.NotNull(result);
            Assert.Equal("Teste", result.Title);
            Assert.Equal(timestamp, result.ReleaseDate);
            Assert.Equal("en", result.Language);
            Assert.Equal("Teste Overview", result.Overview);
            Assert.Equal(genres, result.Genres);
        }

        [Fact]
        public void CreateMovieDTO_NullExtDBMovie_ShouldCreateEmptyMovieDTO()
        {
            var factory = new MovieFactory();
            ExtDBMovie dbMovie = null;

            var result = factory.CreateMovieDTO(dbMovie);

            Assert.NotNull(result);
        }
    }
}
