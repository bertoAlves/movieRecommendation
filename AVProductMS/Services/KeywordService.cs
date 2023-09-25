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
    /// Keyword Service
    /// </summary>
    public class KeywordService : IKeywordService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IOptions<MovieDBApiOptions> _movieDbApiOptions;

        /// <summary>
        /// Movie Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="factory"></param>
        /// <param name="movieDbApiOptions"></param>
        public KeywordService(IHttpClientWrapper httpClient, IOptions<MovieDBApiOptions> movieDbApiOptions)
        {
            _httpClient = httpClient;
            _movieDbApiOptions = movieDbApiOptions;
        }

        /// <summary>
        /// Get Movie Keywords
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<KeywordDTO>> GetMovieKeywords(int movieID)
        { 
            var baseUri = _movieDbApiOptions.Value.BaseUri;
            var keywordsEndpoint = _movieDbApiOptions.Value.KeywordsEndpoint;
            keywordsEndpoint = keywordsEndpoint.Replace("#MOVIE_ID#", movieID.ToString());

            var requestUri = $"{baseUri}{keywordsEndpoint}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();

                var extMovies = JsonConvert.DeserializeObject<ExtDBMovieKeywords>(movieDetails);

                return extMovies?.Keywords?.Select(x => new KeywordDTO { Name = x.Name, Id = x.Id }) ?? new List<KeywordDTO>();
            }
            else
            {
                throw new HttpRequestException($"MovieDB API request failed with status code {response.StatusCode}");
            }
        }
    }
}
