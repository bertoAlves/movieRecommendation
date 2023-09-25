using Common.CinemaMS.DTO;

namespace APIGateway.Factories.Interfaces
{
    /// <summary>
    /// Genre Factory
    /// </summary>
    public interface IGenreFactory
    {
        /// <summary>
        /// Beezy Genre
        /// </summary>
        /// <param name="genres"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> MapBeezyGenre(IEnumerable<GenreDTO> genres);
    }
}
