using CinemaMS.Factories.Interfaces;
using CinemaMS.Factories;
using CinemaMS.Models;

namespace CinemaMS.Test.Factories
{
    public class BuildGenreDTOTest
    {
        private readonly IBuildGenreDTO _genreDTOBuilder;

        public BuildGenreDTOTest()
        {
            _genreDTOBuilder = new BuildGenreDTO();
        }

        [Fact]
        public void CreateGenreDTO_ShouldCreateGenreDTO()
        {
            var genre = new Genre
            {
                Id = 1,
                Name = "Action",
            };

            var genreDTO = _genreDTOBuilder.CreateGenreDTO(genre);

            Assert.NotNull(genreDTO);
            Assert.Equal(genre.Id, genreDTO.Id);
            Assert.Equal(genre.Name, genreDTO.Name);
        }

        [Fact]
        public void CreateGenreDTO_NullGenre_ShouldReturnNotNullGenreDTO()
        {
            Genre genre = null;

            var genreDTO = _genreDTOBuilder.CreateGenreDTO(genre);

            Assert.NotNull(genreDTO);
        }

        [Fact]
        public void CreateGenreDTO_EmptyNameGenre_ShouldCreateGenreDTOWithEmptyName()
        {
            var genre = new Genre
            {
                Id = 2,
                Name = string.Empty,
            };

            var genreDTO = _genreDTOBuilder.CreateGenreDTO(genre);

            Assert.NotNull(genreDTO);
            Assert.Equal(genre.Id, genreDTO.Id);
            Assert.Equal(string.Empty, genreDTO.Name);
        }

        [Fact]
        public void CreateGenreDTO_SpecialCharactersGenre_ShouldCreateGenreDTOWithSpecialCharacters()
        {
            var genre = new Genre
            {
                Id = 3,
                Name = "_*!genre_|\\\\|///",
            };

            var genreDTO = _genreDTOBuilder.CreateGenreDTO(genre);

            Assert.NotNull(genreDTO);
            Assert.Equal(genre.Id, genreDTO.Id);
            Assert.Equal("_*!genre_|\\\\|///", genreDTO.Name);
        }
    }
}
