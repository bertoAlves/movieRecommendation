using APIGateway.Configuration;
using APIGateway.DTO.AVProduct;
using APIGateway.Services.Interfaces;
using Common.AVProductMS.DTO;
using Common.CinemaMS.DTO;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace APIGateway.Services
{
    /// <summary>
    /// AVProduct MS Service
    /// </summary>
    public class AVProductMSService : IAVProductMSService
    { 

        private readonly IHttpClientWrapper _httpClient;
        private readonly IOptions<MicroservicesOptions> _microservicesOptions;

        /// <summary>
        /// AVProduct MS Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="microservicesOptions"></param>
        public AVProductMSService(IHttpClientWrapper httpClient, IOptions<MicroservicesOptions> microservicesOptions)
        {
            _httpClient = httpClient;
            _microservicesOptions = microservicesOptions;
        }

        public Task<IEnumerable<DocumentaryRecommendation>> GetAllTimeDocumentariesRecommendation(string topics)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieRecommendation>> GetAllTimeMoviesRecommendation(string? keywords, string? genre)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TVShowRecommendation>> GetAllTimeTVShowsRecommendation(string keywords, string genres)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieRecommendation>> GetUpcomingMoviesRecommendation(string? keywords, string genres, int maxDate, string? ageRate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Blockbuster Movies
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<MovieDTO>> GetBlockbusterMovies(string genres, int numberWeeks, int bigScreens)
        {
            if(numberWeeks <= 0)
            {
                return new List<MovieDTO>();
            }

            if (bigScreens <= 0)
            {
                return new List<MovieDTO>();
            }

            var baseUri = _microservicesOptions.Value.AVProductMSUri;
            var blockbusterEndpoint = _microservicesOptions.Value.BlockbustersMovies;

            var queryParameters = new List<string>();
            queryParameters.Add($"genres={genres}");
            queryParameters.Add($"numberOfWeeks={numberWeeks}");
            queryParameters.Add($"numberOfBigScreens={bigScreens}");

            var requestUri = $"{baseUri}{blockbusterEndpoint}?{string.Join("&", queryParameters)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            { 
                var movieDetails = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(movieDetails))
                {
                    return new List<MovieDTO>();
                }
                return JsonConvert.DeserializeObject<IEnumerable<MovieDTO>>(movieDetails);
            }
            else
            {
                throw new HttpRequestException($"CinemaMS Blockbuster API request failed with status code {response.StatusCode}");
            }
        }

        /// <summary>
        /// Get Minority Movies
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<MovieDTO>> GetMinorityGenresMovies(string without_genreIds, int numberWeeks, int smallScreens)
        {
            if (numberWeeks <= 0)
            {
                return new List<MovieDTO>();
            }

            if (smallScreens <= 0)
            {
                return new List<MovieDTO>();
            }

            var baseUri = _microservicesOptions.Value.AVProductMSUri;
            var minorityEndpoint = _microservicesOptions.Value.MinorityGenresMovies;

            var queryParameters = new List<string>();
            queryParameters.Add($"withoutgenres={without_genreIds}");
            queryParameters.Add($"numberOfWeeks={numberWeeks}");
            queryParameters.Add($"numberOfSmallScreens={smallScreens}");

            var requestUri = $"{baseUri}{minorityEndpoint}?{string.Join("&", queryParameters)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(movieDetails))
                {
                    return new List<MovieDTO>();
                }
                return JsonConvert.DeserializeObject<IEnumerable<MovieDTO>>(movieDetails);
            }
            else
            {
                throw new HttpRequestException($"CinemaMS Minority API request failed with status code {response.StatusCode}");
            }
        }
    }
}
