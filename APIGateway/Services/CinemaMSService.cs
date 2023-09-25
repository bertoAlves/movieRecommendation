using APIGateway.Configuration;
using APIGateway.Services.Interfaces;
using Common.CinemaMS.DTO;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace APIGateway.Services
{
    /// <summary>
    /// Cinema MS Service
    /// </summary>
    public class CinemaMSService : ICinemaMSService
    { 

        private readonly IHttpClientWrapper _httpClient;
        private readonly IOptions<MicroservicesOptions> _microservicesOptions;

        /// <summary>
        /// Cinema MS Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="microservicesOptions"></param>
        public CinemaMSService(IHttpClientWrapper httpClient, IOptions<MicroservicesOptions> microservicesOptions)
        {
            _httpClient = httpClient;
            _microservicesOptions = microservicesOptions;
        }

        /// <summary>
        /// Get Successful Genres
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<GenreDTO>> GetSuccessfulGenres()
        {
            var baseUri = _microservicesOptions.Value.CinemaMSURi;
            var successfulGenresEndpoint = _microservicesOptions.Value.SuccessufulGenres;

            var requestUri = $"{baseUri}{successfulGenresEndpoint}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var genresDetails = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(genresDetails))
                {
                    return new List<GenreDTO>();
                }
                var genres = JsonConvert.DeserializeObject<IEnumerable<GenreDTO>>(genresDetails);
                return genres;
            }
            else
            {
                throw new HttpRequestException($"CinemaMS API request failed with status code {response.StatusCode}");
            }
        }
    }
}
