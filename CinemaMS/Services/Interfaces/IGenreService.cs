using Common.CinemaMS.DTO;

namespace CinemaMS.Services.Interfaces
{
    /// <summary>
    /// IGenreService
    /// </summary>
    public interface IGenreService
    {
        /// <summary>
        /// Get Most Successful Genres
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GenreDTO>> GetMostSuccessfulGenres();
    }
}
