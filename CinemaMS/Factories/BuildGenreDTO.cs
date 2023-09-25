using CinemaMS.Factories.Interfaces;
using Common.CinemaMS.DTO;
using CinemaMS.Models;

namespace CinemaMS.Factories
{
    /// <summary>
    /// Build GenreDTO
    /// </summary>
    public class BuildGenreDTO : IBuildGenreDTO
    {
        /// <summary>
        /// GenreDTO builder
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>CinemaDTO</returns>
        public GenreDTO CreateGenreDTO(Genre genre)
        {
            if(genre == null) {
                return new GenreDTO();
            };

            return new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name,
            };
        }
    }
}
