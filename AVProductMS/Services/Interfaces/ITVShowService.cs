using Common.AVProductMS.DTO;

namespace AVProduct.Services.Interfaces
{
    public interface ITVShowService
    {
        Task<IEnumerable<TVShowDTO>> GetAllTimeTVShows(string? keywords, string? genres);
    }
}
