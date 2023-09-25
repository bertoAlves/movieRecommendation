using CinemaMS.Models;

namespace CinemaMS.DAL.Interfaces
{
    /// <summary>
    /// ISessionDAL
    /// </summary>
    public interface ISessionDAL
    {
        /// <summary>
        /// GetMovieSessionAverage
        /// </summary>
        /// <param name="roomIds"></param>
        /// <returns></returns>
        Task<IEnumerable<MovieSessionAverage>> GetMovieSessionAverage(IEnumerable<int> roomIds);
    }
}
