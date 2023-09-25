using Common.AVProductMS.DTO;
using Microsoft.Extensions.Options;
using AVProduct.Configuration;
using AVProduct.DTO;
using AVProduct.Factories.Interfaces;
using AVProduct.Services.Interfaces;
using Newtonsoft.Json;
using Common.HttpClientWrapper;

namespace AVProduct.Services
{
    /// <summary>
    /// Movie Details Service
    /// </summary>
    public class MovieDetailsService : IMovieDetailsService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IMovieDetailsFactory _detailsFactory;
        private readonly IOptions<MovieDBApiOptions> _movieDbApiOptions;

        /// <summary>
        /// Movie Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="detailsFactory"></param>
        /// <param name="movieDbApiOptions"></param>
        public MovieDetailsService(IHttpClientWrapper httpClient, IMovieDetailsFactory detailsFactory, IOptions<MovieDBApiOptions> movieDbApiOptions)
        {
            _httpClient = httpClient;
            _detailsFactory = detailsFactory;
            _movieDbApiOptions = movieDbApiOptions;
        }

        /// <summary>
        /// Get Movie Details
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<MovieDetailsDTO> GetMovieDetails(int movieID)
        {
            var baseUri = _movieDbApiOptions.Value.BaseUri;
            var detailsEndpoint = _movieDbApiOptions.Value.MovieDetailsEndpoint;
            detailsEndpoint = detailsEndpoint.Replace("#MOVIE_ID#", movieID.ToString());
            var detailsParams = _movieDbApiOptions.Value.MovieDetailsParams;

            var requestUri = $"{baseUri}{detailsEndpoint}?{detailsParams}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();

                var extMovies = JsonConvert.DeserializeObject<ExtDBMovieDetails>(movieDetails);

                return _detailsFactory.CreateMovieDetailsDTO(extMovies);
            }
            else
            {
                throw new HttpRequestException($"MovieDB API request failed with status code {response.StatusCode}");
            }
        }

    }
}
