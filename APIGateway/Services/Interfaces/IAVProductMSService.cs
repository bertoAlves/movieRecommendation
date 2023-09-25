using APIGateway.DTO.AVProduct;
using Common.AVProductMS.DTO;

namespace APIGateway.Services.Interfaces
{
    public interface IAVProductMSService
    {
        Task<IEnumerable<MovieDTO>> GetBlockbusterMovies(string genres, int numberWeeks, int bigScreens);

        Task<IEnumerable<MovieDTO>> GetMinorityGenresMovies(string without_genreIds, int numberWeeks, int smallScreens);

        Task<IEnumerable<MovieRecommendation>> GetAllTimeMoviesRecommendation(string? keywords, string? genre);

        Task<IEnumerable<MovieRecommendation>> GetUpcomingMoviesRecommendation(string? keywords, string genres, int maxDate, string? ageRate);

        Task<IEnumerable<TVShowRecommendation>> GetAllTimeTVShowsRecommendation(string keywords, string genres);

        Task<IEnumerable<DocumentaryRecommendation>> GetAllTimeDocumentariesRecommendation(string topics);
    }

}
