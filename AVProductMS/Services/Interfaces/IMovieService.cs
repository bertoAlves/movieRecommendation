using Common.AVProductMS.DTO;

namespace AVProduct.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetAllTimeMovies(string? keywords,string? genres);

        Task<IEnumerable<MovieDTO>> GetUpcomingMovies(string? keywords, string? genres, int daysFromNow, string? ageRate);

        Task<IEnumerable<MovieDTO>> GetBlockbusterMovies(string? genres, int numberOfWeeks, int numberOfBigScreens);

        Task<IEnumerable<MovieDTO>> GetMinorityGenresMovies(string? withoutgenres, int numberOfWeeks, int numberOfBigScreens);
    }
}
