using Common.CinemaMS.DTO;
using CinemaMS.Models;

namespace CinemaMS.Factories.Interfaces
{
    /// <summary>
    /// Build GenreDTO Interface
    /// </summary>
    public interface IBuildGenreDTO
    {
        /// <summary>
        /// Create Genre DTO 
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>CinemaDTO</returns>
        GenreDTO CreateGenreDTO(Genre genre);
    }
}
