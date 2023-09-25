using Common.CinemaMS.DTO;

namespace APIGateway.Services.Interfaces
{
    public interface ICinemaMSService
    {
        Task<IEnumerable<GenreDTO>> GetSuccessfulGenres();
    }

}
