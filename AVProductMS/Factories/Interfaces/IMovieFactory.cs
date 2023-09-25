using Common.AVProductMS.DTO;
using AVProduct.DTO;

namespace AVProduct.Factories.Interfaces
{
    public interface IMovieFactory
    {
        MovieDTO CreateMovieDTO(ExtDBMovie dbMovie);
    }
}
