using CinemaMS.Models;

namespace CinemaMS.DAL.Interfaces
{
    /// <summary>
    /// IGenreDAL
    /// </summary>
    public interface IGenreDAL
    {
        /// <summary>
        /// GetMostSuccessfulGenres
        /// </summary>
        /// <param name="numberSellouts"></param>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetMostSuccessfulGenres(int numberSellouts = 0);

        /// <summary>
        /// GetMostSeatsSoldGenres
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetMostSeatsSoldGenres();
    }
}
