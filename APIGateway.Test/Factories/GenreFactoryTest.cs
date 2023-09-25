using APIGateway.Factories;
using Common.CinemaMS.DTO;

namespace APIGateway.Test.Factories
{
    public class GenreFactoryTest
    {
        [Fact]
        public async Task MapBeezyGenre_ShouldMapGenresToMovieDBGenreIds()
        {
            var genreMappings = new List<GenreMapping>
            {
                new GenreMapping { BeezyGenreId = 1, MovieDBGenreId = 28, GenreName = "Action" },
                new GenreMapping { BeezyGenreId = 2, MovieDBGenreId = 12, GenreName = "Adventure" },
            };

            var genreFactory = new GenreFactory();
            var genresToMap = new List<GenreDTO>
            {
                new GenreDTO { Id = 1, Name = "Action" },
                new GenreDTO { Id = 2, Name = "Adventure" },
            };

            var mappedGenreIds = await genreFactory.MapBeezyGenre(genresToMap);
            Assert.Equal(genreMappings.Select(mapping => mapping.MovieDBGenreId), mappedGenreIds);
        }

        [Fact]
        public async Task MapBeezyGenre_ShouldIgnoreUnknownGenres()
        {
            var genreMappings = new List<GenreMapping>
            {
                new GenreMapping { BeezyGenreId = 1, MovieDBGenreId = 28, GenreName = "Action" },
            };

            var genreFactory = new GenreFactory();
            var genresToMap = new List<GenreDTO>
            {
                new GenreDTO { Id = 1, Name = "Action" },
                new GenreDTO { Id = 3, Name = "UnknownGenre" }
            };

            var mappedGenreIds = await genreFactory.MapBeezyGenre(genresToMap);
            Assert.Single(mappedGenreIds);
            Assert.Equal(new List<int>() { 28 }, mappedGenreIds);
        }
    }
}
