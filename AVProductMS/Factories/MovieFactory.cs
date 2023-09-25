using Common.AVProductMS.DTO;
using AVProduct.DTO;
using AVProduct.Factories.Interfaces;

namespace AVProduct.Factories
{
    /// <summary>
    /// MovieFactory
    /// </summary>
    public class MovieFactory : IMovieFactory
    {
        /// <summary>
        /// CreateMovieDTO
        /// </summary>
        /// <param name="dbMovie"></param>
        /// <returns></returns>
        public MovieDTO CreateMovieDTO(ExtDBMovie dbMovie)
        {
            if (dbMovie == null)
            {
                return new MovieDTO();
            }

            return new MovieDTO { Id = dbMovie.ID, Title = dbMovie.Title, ReleaseDate = dbMovie.ReleaseDate, Language = dbMovie.OriginalLanguage, Overview = dbMovie.Overview, Genres = dbMovie.GenresIDs};
        }
    }
}
