using Common.AVProductMS.DTO;
using Microsoft.Extensions.Options;
using AVProduct.Configuration;
using AVProduct.DTO;
using AVProduct.Factories.Interfaces;
using AVProduct.Services.Interfaces;
using Newtonsoft.Json;
using Common.CinemaMS.DTO;
using Common.HttpClientWrapper;
using Microsoft.Extensions.Caching.Memory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AVProduct.Services
{
    /// <summary>
    /// Movie Service
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IMovieFactory _factory;
        private readonly IOptions<MovieDBApiOptions> _movieDbApiOptions;
        private readonly IKeywordService _keywordService;
        private readonly IMovieDetailsService _movieDetailsService;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Movie Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="factory"></param>
        /// <param name="movieDbApiOptions"></param>
        /// <param name="keywordService"></param>
        /// <param name="movieDetailsService"></param>
        /// <param name="cache"></param>
        public MovieService(IHttpClientWrapper httpClient, 
                            IMovieFactory factory, 
                            IOptions<MovieDBApiOptions> movieDbApiOptions, 
                            IKeywordService keywordService, 
                            IMovieDetailsService movieDetailsService,
                            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _factory = factory;
            _movieDbApiOptions = movieDbApiOptions;
            _keywordService = keywordService;
            _movieDetailsService = movieDetailsService;
            _cache = cache;
        }

        /// <summary>
        /// Get All Time Movies Service Method
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="genres"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<MovieDTO>> GetAllTimeMovies(string? keywords, string? genres)
        {
            var cacheKey = $"GetAllTimeMovies_{keywords}_{genres}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<MovieDTO> cachedMovies))
            {
                return cachedMovies;
            }

            var baseUri = _movieDbApiOptions.Value.BaseUri;
            var trendingEndpoint = _movieDbApiOptions.Value.AllTimeMoviesEndpoint;
            var trendingParams = _movieDbApiOptions.Value.AllTimeMoviesParams;

            var queryParameters = new List<string>();
            queryParameters.Add($"with_genres={genres}");
            queryParameters.Add($"with_keywords={keywords}");

            var requestUri = $"{baseUri}{trendingEndpoint}?{trendingParams}&{string.Join("&", queryParameters)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();

                var extMovies = JsonConvert.DeserializeObject<MovieDBResponse>(movieDetails)?.Results;

                IEnumerable<MovieDTO> movies = extMovies?.Select(extmovie => _factory.CreateMovieDTO(extmovie));

                if (movies is null)
                {
                    return new List<MovieDTO>();
                }

                var movieList = movies.ToList<MovieDTO>();

                foreach (MovieDTO movie in movieList)
                {
                    MovieDetailsDTO dto = await _movieDetailsService.GetMovieDetails(movie.Id);
                    movie.Website = dto.Website;
                    IEnumerable<KeywordDTO> list = await _keywordService.GetMovieKeywords(movie.Id);
                    movie.Keywords = list.ToList();
                }

                _cache.Set(cacheKey, movieList, TimeSpan.FromMinutes(5));

                return movieList;
            }
            else
            {
                throw new HttpRequestException($"MovieDB API request failed with status code {response.StatusCode}");
            }
        }

        /// <summary>
        /// Get Upcoming Movies Service Method
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="genres"></param>
        /// <param name="daysFromNow"></param>
        /// <param name="ageRate"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<MovieDTO>> GetUpcomingMovies(string? keywords, string? genres, int daysFromNow, string? ageRate)
        {
            if (daysFromNow <= 0)
            {
                return new List<MovieDTO>();
            }

            var cacheKey = $"GetUpcomingMovies_{keywords}_{genres}_{daysFromNow}_{ageRate}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<MovieDTO> cachedMovies))
            {
                return cachedMovies;
            }

            var baseUri = _movieDbApiOptions.Value.BaseUri;
            var upcomingEndpoint = _movieDbApiOptions.Value.UpcomingMoviesEndpoint;
            var upcomingParams = _movieDbApiOptions.Value.UpcomingMoviesParams;

            var queryParameters = new List<string>();
            DateTime date = DateTime.Now.AddDays(daysFromNow);

            queryParameters.Add($"with_genres={genres}");
            queryParameters.Add($"with_keywords={keywords}");
            queryParameters.Add($"primary_release_date.gte={DateTime.Now.ToString("yyyy-MM-dd")}");
            queryParameters.Add($"primary_release_date.lte={date.ToString("yyyy-MM-dd")}");

            var requestUri = $"{baseUri}{upcomingEndpoint}?{upcomingParams}&{string.Join("&", queryParameters)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();

                var extMovies = JsonConvert.DeserializeObject<MovieDBResponse>(movieDetails)?.Results;

                IEnumerable<MovieDTO> movies = extMovies?.Select(extmovie => _factory.CreateMovieDTO(extmovie));

                if (movies is null)
                {
                    return new List<MovieDTO>();
                }

                var movieList = movies.ToList<MovieDTO>();

                foreach (MovieDTO movie in movieList)
                {
                    MovieDetailsDTO dto = await _movieDetailsService.GetMovieDetails(movie.Id);
                    movie.Website = dto.Website;
                    IEnumerable<KeywordDTO> list = await _keywordService.GetMovieKeywords(movie.Id);
                    movie.Keywords = list.ToList();
                }

                _cache.Set(cacheKey, movieList, TimeSpan.FromMinutes(5));

                return movieList;
            }
            else
            {
                throw new HttpRequestException($"MovieDB API request failed with status code {response.StatusCode}");
            }
        }


        /// <summary>
        /// Get Blockbuster Movies
        /// </summary>
        /// <param name="genres"></param>
        /// <param name="numberOfWeeks"></param>
        /// <param name="numberOfBigScreens"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<MovieDTO>> GetBlockbusterMovies(string? genres, int numberOfWeeks, int numberOfBigScreens)
        {
            if (numberOfWeeks <= 0)
            {
                return new List<MovieDTO>();
            }

            if (numberOfBigScreens <= 0)
            {
                return new List<MovieDTO>();
            }

            var cacheKey = $"GetBlockbusterMovies_{genres}_{numberOfWeeks}_{numberOfBigScreens}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<MovieDTO> cachedMovies))
            {
                return cachedMovies;
            }

            var baseUri = _movieDbApiOptions.Value.BaseUri;
            var blockbusterEndpoint = _movieDbApiOptions.Value.BlockbustersEndpoint;
            var maxNumber = _movieDbApiOptions.Value.MaxNumberOfMovies;
            var blockbusterParams = _movieDbApiOptions.Value.BlockbustersParams;

            var queryParameters = new List<string>();
            queryParameters.Add($"with_genres={genres}");

            var requestUri = $"{baseUri}{blockbusterEndpoint}?{blockbusterParams}&{string.Join("&", queryParameters)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();

                var numberResults = 0;
                if (maxNumber < (numberOfWeeks * numberOfBigScreens))
                {
                    numberResults = maxNumber;
                }
                else {
                    numberResults = numberOfWeeks * numberOfBigScreens;
                }

                var extMovies = JsonConvert.DeserializeObject<MovieDBResponse>(movieDetails)?.Results.Take(numberResults);

                IEnumerable<MovieDTO> movies = extMovies?.Select(extmovie => _factory.CreateMovieDTO(extmovie));

                if (movies is null)
                {
                    return new List<MovieDTO>();
                }

                var movieList = movies.ToList<MovieDTO>();

                foreach (MovieDTO movie in movieList)
                {
                    MovieDetailsDTO dto = await _movieDetailsService.GetMovieDetails(movie.Id);
                    movie.Website = dto.Website;
                    IEnumerable<KeywordDTO> list = await _keywordService.GetMovieKeywords(movie.Id);
                    movie.Keywords = list.ToList();
                }

                _cache.Set(cacheKey, movieList, TimeSpan.FromMinutes(5));

                return movieList;
            }
            else
            {
                throw new HttpRequestException($"MovieDB API request failed with status code {response.StatusCode}");
            }
        }

        /// <summary>
        /// Get Minority Genres Movies
        /// </summary>
        /// <param name="withoutgenres"></param>
        /// <param name="numberOfWeeks"></param>
        /// <param name="numberOfSmallScreens"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<MovieDTO>> GetMinorityGenresMovies(string? withoutgenres, int numberOfWeeks, int numberOfSmallScreens)
        {
            if (numberOfWeeks <= 0)
            {
                return new List<MovieDTO>();
            }

            if (numberOfSmallScreens <= 0)
            {
                return new List<MovieDTO>();
            }

            var cacheKey = $"GetMinorityGenresMoviess_{withoutgenres}_{numberOfWeeks}_{numberOfSmallScreens}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<MovieDTO> cachedMovies))
            {
                return cachedMovies;
            }

            var baseUri = _movieDbApiOptions.Value.BaseUri;
            var minotiryEndpoint = _movieDbApiOptions.Value.MinorityGenresEndpoint;
            var maxNumber = _movieDbApiOptions.Value.MaxNumberOfMovies;
            var minotiryParams = _movieDbApiOptions.Value.MinorityGenresParams;

            var queryParameters = new List<string>();
            queryParameters.Add($"without_genres={withoutgenres}");

            var requestUri = $"{baseUri}{minotiryEndpoint}?{minotiryParams}&{string.Join("&", queryParameters)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var movieDetails = await response.Content.ReadAsStringAsync();

                var numberResults = 0;
                if (maxNumber < (numberOfWeeks * numberOfSmallScreens))
                {
                    numberResults = maxNumber;
                }
                else
                {
                    numberResults = numberOfWeeks * numberOfSmallScreens;
                }

                var extMovies = JsonConvert.DeserializeObject<MovieDBResponse>(movieDetails)?.Results.Take(numberResults);

                IEnumerable<MovieDTO> movies = extMovies?.Select(extmovie => _factory.CreateMovieDTO(extmovie));

                if (movies is null)
                {
                    return new List<MovieDTO>();
                }
                var movieList = movies.ToList<MovieDTO>();

                foreach (MovieDTO movie in movieList)
                {
                    MovieDetailsDTO dto = await _movieDetailsService.GetMovieDetails(movie.Id);
                    movie.Website = dto.Website;
                    IEnumerable<KeywordDTO> list = await _keywordService.GetMovieKeywords(movie.Id);
                    movie.Keywords = list.ToList();
                }

                _cache.Set(cacheKey, movieList, TimeSpan.FromMinutes(5));

                return movieList;
            }
            else
            {
                throw new HttpRequestException($"MovieDB API request failed with status code {response.StatusCode}");
            }
        }
    }
}
