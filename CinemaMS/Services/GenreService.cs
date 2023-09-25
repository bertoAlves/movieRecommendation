using CinemaMS.Configuration;
using CinemaMS.DAL.Interfaces;
using CinemaMS.Factories.Interfaces;
using CinemaMS.Models;
using CinemaMS.Services.Interfaces;
using Common.CinemaMS.DTO;
using Common.Exceptions;
using Microsoft.Extensions.Options;

namespace CinemaMS.Services
{
    /// <summary>
    /// Genre Service
    /// </summary>
    public class GenreService : IGenreService
    {

        private readonly IGenreDAL _genreDAL;
        private readonly IBuildGenreDTO _factory;
        private readonly IOptions<SuccessfulGenresOptions> _successfulGenresOptions;


        /// <summary>
        /// Genre Service
        /// </summary>
        /// <param name="genreDAL"></param>
        /// <param name="factory"></param>
        public GenreService(IGenreDAL genreDAL, IBuildGenreDTO factory, IOptions<SuccessfulGenresOptions> successfulGenresOptions)
        {
            _genreDAL = genreDAL;
            _factory = factory;
            _successfulGenresOptions = successfulGenresOptions;
        }

        /// <summary>
        /// Get Most Successful Genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<GenreDTO>> GetMostSuccessfulGenres()
        {
            IEnumerable<Genre> genres = null;

            var algorithm = _successfulGenresOptions.Value.Algorithm ?? "SELL-OUTS";

            switch (algorithm)
            {
                case "SELL-OUTS":
                    var minSellouts = _successfulGenresOptions.Value.MinNumberSellouts ?? 0;
                    genres = await _genreDAL.GetMostSuccessfulGenres(minSellouts);
                    break;
                case "MOST_SEATS_SOLD":
                    genres = await _genreDAL.GetMostSeatsSoldGenres();
                    break;
                // Add more cases for other algorithms if needed in the future
                default:
                    throw new UnsupportedAlgorithmException(algorithm);
            }

            return genres?.Select(x => _factory.CreateGenreDTO(x)) ?? Enumerable.Empty<GenreDTO>();
        }
    }
}
