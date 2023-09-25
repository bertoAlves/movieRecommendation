using Common.AVProductMS.DTO;

namespace AVProduct.Services.Interfaces
{
    public interface IKeywordService
    {
        Task<IEnumerable<KeywordDTO>> GetMovieKeywords(int movieID);
    }
}
