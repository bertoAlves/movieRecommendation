using Common.AVProductMS.DTO;
using Microsoft.Extensions.Options;
using AVProduct.Configuration;
using AVProduct.Factories.Interfaces;
using AVProduct.Services.Interfaces;
using Common.HttpClientWrapper;

namespace AVProduct.Services
{
    /// <summary>
    /// TVShow Service
    /// </summary>
    public class TVShowService : ITVShowService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ITVShowFactory _factory;
        private readonly IOptions<MovieDBApiOptions> _movieDbApiOptions;

        /// <summary>
        /// TVShow Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="factory"></param>
        /// <param name="movieDbApiOptions"></param>
        public TVShowService(IHttpClientWrapper httpClient, ITVShowFactory factory, IOptions<MovieDBApiOptions> movieDbApiOptions)
        {
            _httpClient = httpClient;
            _factory = factory;
            _movieDbApiOptions = movieDbApiOptions;
        }


        /// <summary>
        /// Get All Time TVShows
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="genres"></param>
        /// <returns>List of TVShows</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<TVShowDTO>> GetAllTimeTVShows(string? keywords, string? genres)
        {
            //Recommend Mr Robot
            throw new NotImplementedException();
        }
    }
}
