using APIGateway.DTO.AVProduct;
using Common.AVProductMS.DTO;
using Common.CinemaMS.DTO;

namespace APIGateway.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<MovieRecommendation>> GetAllTimeMoviesRecommendation(string? keywords, string? genre);

        Task<IEnumerable<MovieRecommendation>> GetUpcomingMoviesRecommendation(string? keywords, string? genres, int maxDate);

        Task<IEnumerable<MovieRecommendation>> GetUpcomingMoviesRecommendationByAgeRate(string genres, int maxDate, string ageRate);

        Task<IEnumerable<TVShowRecommendation>> GetAllTimeTVShowsRecommendation(string keywords, string genres);

        Task<IEnumerable<DocumentaryRecommendation>> GetAllTimeDocumentariesRecommendation(string topics);

        Task<Billboard> GetSuggestedBillboard(int numberScreens, int numberWeeks);

        Task<IntelligentBillboard> GetIntelligentBillboard(int numberWeeks, int bigScreens, int smallScreens, bool useSuccessfulGenres);
    }

}
