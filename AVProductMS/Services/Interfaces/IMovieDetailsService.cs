using Common.AVProductMS.DTO;

namespace AVProduct.Services.Interfaces
{
    public interface IMovieDetailsService
    {
        Task<MovieDetailsDTO> GetMovieDetails(int movieID);
    }
}
