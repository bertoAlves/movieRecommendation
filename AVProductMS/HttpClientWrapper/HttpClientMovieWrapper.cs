namespace Common.HttpClientWrapper
{
    public class HttpClientMovieWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientMovieWrapper(IHttpClientFactory httpClientfactory)
        {
            _httpClient = httpClientfactory.CreateClient("MovieDbApi");
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }
    }
}
