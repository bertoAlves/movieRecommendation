namespace Common.HttpClientWrapper
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(IHttpClientFactory httpClientfactory)
        {
            _httpClient = httpClientfactory.CreateClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }
    }
}
