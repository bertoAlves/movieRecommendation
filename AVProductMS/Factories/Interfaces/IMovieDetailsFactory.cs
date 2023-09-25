using Common.AVProductMS.DTO;
using AVProduct.DTO;

namespace AVProduct.Factories.Interfaces
{
    public interface IMovieDetailsFactory
    {
        MovieDetailsDTO CreateMovieDetailsDTO(ExtDBMovieDetails? dbMovie);
    }
}
