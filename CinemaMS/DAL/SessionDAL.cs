using CinemaMS.DAL.Interfaces;
using CinemaMS.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaMS.DAL
{
    /// <summary>
    /// SessionDAL
    /// </summary>
    public class SessionDAL : ISessionDAL
    { 
        private CinemaContext _context;

        /// <summary>
        /// SessionDAL
        /// </summary>
        /// <param name="context"></param>
        public SessionDAL(CinemaContext context) => _context = context;

        /// <summary>
        /// GetMovieSessionAverage
        /// </summary>
        /// <param name="roomIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MovieSessionAverage>> GetMovieSessionAverage(IEnumerable<int> roomIds)
        {
             return await _context.Sessions
                                .Where(session => roomIds.Contains(session.RoomId))
                                .GroupBy(session => session.MovieId)
                                .Select(group => new MovieSessionAverage
                                {
                                    MovieID = group.Key,
                                    AverageSeatsSold = group.Average(session => session.SeatsSold)
                                })
                                .OrderByDescending(x => x.AverageSeatsSold)
                                .ToListAsync();
        }
    }
}
