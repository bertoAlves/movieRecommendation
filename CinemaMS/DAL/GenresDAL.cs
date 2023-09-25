using CinemaMS.DAL.Interfaces;
using CinemaMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CinemaMS.DAL
{
    /// <summary>
    /// GenreDAL
    /// </summary>
    public class GenreDAL : IGenreDAL
    { 
        private CinemaContext _context;

        /// <summary>
        /// GenreDAL
        /// </summary>
        /// <param name="context"></param>
        public GenreDAL(CinemaContext context) { 
            _context = context; 
        }

        /// <summary>
        /// Get Most Successful Genres
        /// </summary>
        /// <param name="numberSellouts"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Genre>> GetMostSuccessfulGenres(int numberSellouts = 0)
        {

            /*  GET Movies that have sold-out rooms
             * 
                SELECT sellouts.Id
                    FROM (
                        SELECT m.Id, COUNT(*) AS number_sellouts
                        FROM Movie m
                        LEFT JOIN Session s ON s.MovieId = m.Id
                        LEFT JOIN Room r ON s.RoomId = r.Id
                        WHERE s.SeatsSold = r.Seats
                        GROUP BY m.Id
                    ) sellouts
                WHERE number_sellouts > :numberSellouts
             * 
             */
            var movieSellouts = from movie in _context.Movies
                                join session in _context.Sessions on movie.Id equals session.MovieId
                                join room in _context.Rooms on session.RoomId equals room.Id
                                where session.SeatsSold == room.Seats
                                group movie by movie.Id into g
                                where g.Count() > numberSellouts
                                select g.Key;

            var selloutsIds = from movie in _context.Movies
                              where movieSellouts.Contains(movie.Id)
                              select movie.Id;

            /*  GET Genres IDs from movies that have sold-out
             * 
                SELECT g.Id, g.Name
                    FROM Genre g
                    WHERE g.Id IN (SELECT mg.GenreId
                                    FROM MovieGenre mg
                                    WHERE mg.MovieId IN ( :selloutsIds)
             * 
             */
            var genreIds = from movieGenres in _context.MovieGenres
                           where selloutsIds.Contains(movieGenres.MovieId)
                           select movieGenres.GenreId;

            var res_genres = from genres in _context.Genres
                         where genreIds.Contains(genres.Id)
                         select genres;

            return res_genres;
        }

        /// <summary>
        /// Get Most Seats Sold Genres
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Genre>> GetMostSeatsSoldGenres()
        {
            var movies = from movie in _context.Movies
                        join session in _context.Sessions on movie.Id equals session.MovieId
                        join room in _context.Rooms on session.RoomId equals room.Id
                        group session by movie.Id into g
                        orderby g.Sum(s => s.SeatsSold) descending
                        select g.Key;

            var movieMostSeatsSold = movies.FirstOrDefault();

            var genreIds = from movieGenres in _context.MovieGenres
                           where movieMostSeatsSold.Equals(movieGenres.MovieId)
                           select movieGenres.GenreId;

            var res_genres = from genres in _context.Genres
                             where genreIds.Contains(genres.Id)
                             select genres;

            return res_genres;
        }
    }
}
