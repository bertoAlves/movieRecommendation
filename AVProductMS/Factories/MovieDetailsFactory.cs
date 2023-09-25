using Common.AVProductMS.DTO;
using AVProduct.DTO;
using AVProduct.Factories.Interfaces;

namespace AVProduct.Factories
{
    /// <summary>
    /// MovieDetailsFactory
    /// </summary>
    public class MovieDetailsFactory : IMovieDetailsFactory
    {
        /// <summary>
        /// CreateMovieDetailsDTO
        /// </summary>
        /// <param name="dbMovie"></param>
        /// <returns></returns>
        public MovieDetailsDTO CreateMovieDetailsDTO(ExtDBMovieDetails? dbMovie)
        {
            if (dbMovie == null)
            {
                return new MovieDetailsDTO();
            }

            return new MovieDetailsDTO { Website = dbMovie.Homepage};
        }
    }
}
